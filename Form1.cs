using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;








namespace BlueAzul
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string datos = "";
        public int read = 0;
        private void button1_Click(object sender, EventArgs e)
        {



            BluetoothClient cliente = new BluetoothClient();

            var infos = cliente.DiscoverDevices();

            var InfoDevices = infos.ToArray();

            foreach (var Infort in InfoDevices)
            {
                listBox1.Items.Add("Nombre: "+Infort.DeviceName+ " Addres:"+ Infort.DeviceAddress);
                

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Task tarea = new Task(Lectura);
            tarea.Start();
            richTextBox1.Text = "*********A la espera de conexion********";
            

        }



        private async void Lectura()
        {

            try
            {

                Guid serviceClass = BluetoothService.SerialPort;
                BluetoothListener serv = new BluetoothListener(serviceClass);
                serv.Start();
                MessageBox.Show("Iniciado Servicio");
                

                BluetoothClient conn = serv.AcceptBluetoothClient();

                MessageBox.Show("¡Conectado!");
                pictureBox1.BackColor = Color.GreenYellow;
                
                Stream xtrem = conn.GetStream();



                while (conn.Connected)
                {
                    //handle server connection
                    int read;
                    string dato;
                    byte[] Received = new byte[1024];
                    read = await xtrem.ReadAsync(Received, 0, Received.Length);

                    if (read > 0)
                    {
                        dato = Encoding.ASCII.GetString(Received);
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() => richTextBox1.Text = dato));


                        }



                    }


                    xtrem.Flush();


                }


            }

            catch (Exception w)
            {

                MessageBox.Show("Error, encende tu Bluetooh " + w);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Application.Exit();
        }
    }

}

