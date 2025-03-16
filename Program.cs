using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

class GravitySimulation : Form
{
    private Timer timer;
    private float y = 50; // Initial height (m)
    private float velocity = 0; // Initial velocity (m/s)
    private float acceleration = 9.81f; // Gravity (m/s²)
    private float mass = 1.0f; // Mass (kg)
    private float energyLoss = 0.8f; // Bounce energy retention (0-1)
    private float timeStep = 0.016f; // 60 FPS (16ms per frame)

    private List<PointF> velocityGraph = new List<PointF>();
    private List<PointF> accelerationGraph = new List<PointF>();
    private int graphWidth = 300, graphHeight = 150;

    public GravitySimulation()
    {
        this.Text = "Scientific Gravity Simulation";
        this.Size = new Size(600, 700);
        this.DoubleBuffered = true;

        timer = new Timer();
        timer.Interval = 16; // 60 FPS
        timer.Tick += UpdateSimulation;
        timer.Start();
    }

    private void UpdateSimulation(object sender, EventArgs e)
    {
        velocity += acceleration * timeStep; // Update velocity
        y += velocity * timeStep; // Update position

        // Check for ground collision
        if (y > this.ClientSize.Height - 50)
        {
            y = this.ClientSize.Height - 50;
            velocity *= -energyLoss; // Bounce with energy loss
        }

        // Store velocity and acceleration for graphing
        if (velocityGraph.Count > graphWidth)
        {
            velocityGraph.RemoveAt(0);
            accelerationGraph.RemoveAt(0);
        }
        velocityGraph.Add(new PointF(velocityGraph.Count, velocity));
        accelerationGraph.Add(new PointF(accelerationGraph.Count, acceleration));

        this.Invalidate(); // Redraw
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Draw falling object
        g.FillEllipse(Brushes.Blue, 150, (int)y, 50, 50);

        // Draw velocity graph
        DrawGraph(g, velocityGraph, "Velocity (m/s)", Brushes.Red, 50);

        // Draw acceleration graph
        DrawGraph(g, accelerationGraph, "Acceleration (m/s²)", Brushes.Green, 250);
    }

    private void DrawGraph(Graphics g, List<PointF> data, string label, Brush color, int yOffset)
    {
        g.DrawString(label, Font, Brushes.White, 350, yOffset - 20);
        if (data.Count < 2) return;

        for (int i = 1; i < data.Count; i++)
        {
            g.DrawLine(new Pen(color, 2),
                350 + (i - 1), yOffset - data[i - 1].Y * 5,
                350 + i, yOffset - data[i].Y * 5);
        }
    }

    static void Main()
    {
        Application.Run(new GravitySimulation());
    }
}
