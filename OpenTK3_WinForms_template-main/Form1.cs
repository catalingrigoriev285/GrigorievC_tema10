using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using OpenTK3_StandardTemplate_WinForms.helpers;
using OpenTK3_StandardTemplate_WinForms.objects;

namespace OpenTK3_StandardTemplate_WinForms
{
    public partial class MainForm : Form
    {
        private Axes mainAxis;
        private Camera cam;
        private Scene scene;

        private Vehicle vehicle;

        private Bee bee;
        private Grid grid;

        private Point mousePosition;

        public MainForm()
        {   
            // general init
            InitializeComponent();

            // init VIEWPORT
            scene = new Scene();

            scene.GetViewport().Load += new EventHandler(this.mainViewport_Load);
            scene.GetViewport().Paint += new PaintEventHandler(this.mainViewport_Paint);
            scene.GetViewport().MouseMove += new MouseEventHandler(this.mainViewport_MouseMove);

            this.Controls.Add(scene.GetViewport());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // init RNG
            Randomizer.Init();

            // init CAMERA/EYE
            cam = new Camera(scene.GetViewport());

            // init AXES
            mainAxis = new Axes(showAxes.Checked);

            grid = new Grid();

            Timer timer = new Timer();
            //timer.Interval = 100;
            timer.Interval = 30; 
            
            timer.Tick += (s, args) =>
            {
                if (vehicle != null)
                {
                    float speed = 2.0f;
                    Vector3 currentPos = vehicle.Position;

                    vehicle.Position = new Vector3(currentPos.X + speed, currentPos.Y, currentPos.Z);

                    if (vehicle.Position.X > 150.0f)
                    {
                        vehicle.Position = new Vector3(-100.0f, currentPos.Y, currentPos.Z);
                    }
                }
                
                if (bee != null)
                {
                    bee.UpdateAnimation(); 
                }

                scene.Invalidate();
            };
            timer.Start();
        }

        private void showAxes_CheckedChanged(object sender, EventArgs e)
        {
            mainAxis.SetVisibility(showAxes.Checked);
        }

        private void changeBackground_Click(object sender, EventArgs e)
        {
            GL.ClearColor(Randomizer.GetRandomColor());

            scene.Invalidate();
        }

        private void resetScene_Click(object sender, EventArgs e)
        {
            showAxes.Checked = true;
            mainAxis.SetVisibility(showAxes.Checked);
            scene.Reset();
            cam.Reset();

            scene.Invalidate();
        }

        private void mainViewport_Load(object sender, EventArgs e)
        {
            scene.Reset();
            scene.Load();
            
            vehicle = new Vehicle("../../assets/vehicle/source/vehicle.obj", Color.Red);
            vehicle.Rotation = new Vector3(0, 90, 0);

            vehicle.LoadTexture("../../assets/vehicle/textures/Body_Metallic.png");
            vehicle.Position = new Vector3(0, 0, 50);

            bee = new Bee(new Vector3(40, 32, 90));
            bee.Rotation = new Vector3(0, -90, 0);
        }

        private void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = new Point(e.X, e.Y);
            scene.Invalidate();
        }

        private void mainViewport_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            cam.SetView();

            // GRAPHICS PAYLOAD
            mainAxis.Draw();
            scene.Draw();

            if(grid != null)
            {
                grid.Draw();
            }

            if (vehicle != null)
            {
                vehicle.Draw();
            }

            if (bee != null)
            {
                bee.Draw();
            }

            scene.GetViewport().SwapBuffers();
        }


    }
}
