using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace bater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            var devolver = new string[1];
            devolver = readFile("bater\\CurrentSession.txt", "Problem reading CurrentSession.txt: ");
            //Form1.ActiveForm.Text = devolver[1];
            MessageBox.Show("Welcome " + devolver[0]);

            InitializeComponent(devolver[0]);
            InitializeSessions(); // to add the sessions from the session file
        }

        // open session button
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to change the current session to " + sessions.Text + "?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Conitnuo borrando así que no hago nada
                string path2 = "bater\\CurrentSession.txt";
                var Arguments2 = new string[1];
                Arguments2[0] = sessions.Text;
                writeFile(path2, Arguments2, "Your session has been changed to " + sessions.Text, false);
                Form1.ActiveForm.Text = sessions.Text;
            }
            else
            {
                return;
            }
            string path = @"bater\openSession.bat";
            var Arguments = new string[1];
            Arguments[0] = sessions.Text;
            ExecutarScript(path, Arguments);
        }

        //Create Session button
        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to create the session: " + textBox1.Text, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Conitnuo borrando así que no hago nada
            }
            else
            {
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter a valid name for the session");
                return;
            }

            var cantidad = readFile("bater\\sessions.txt", "Problem reading sessions.txt: ").Length;
            var devolver = new string[cantidad];
            devolver = readFile("bater\\sessions.txt", "Problem reading sessions.txt: ");
            for (var i = 0; i < devolver.Length; i++)
            {
                if (devolver[i] == textBox1.Text)
                {
                    MessageBox.Show("This session already exists. Please try with another name.");
                    return;
                }
            }

            // Create the session in the textbox
            string path = "bater\\sessions.txt";
            var Arguments = new string[1];
            Arguments[0] = textBox1.Text;
            writeFile(path, Arguments, "New session created: " + textBox1.Text, true);

            //Refresh the sessions to show
            sessions.Items.Clear();
            InitializeSessions();

            // executes createSession.bat
            path = @"bater\createSession.bat";
            Arguments = new string[1];
            Arguments[0] = textBox1.Text;
            ExecutarScript(path, Arguments);

        }

        //delete Session button
        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to delete the session: " + sessions.Text + "?.\n This action can't be undone.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Conitnuo borrando así que no hago nada
            }
            else
            {
                return;
            }
            if (sessions.Text == "")
            {
                return;
            }
            if (sessions.Text == Form1.ActiveForm.Text)
            {
                MessageBox.Show("You can't delete the current session. Change to another one and try it again.");
                return;
            }


            var cantidad = readFile("bater\\sessions.txt", "Problem reading sessions.txt: ").Length;
            var devolver = new string[cantidad];
            devolver = readFile("bater\\sessions.txt", "Problem reading sessions.txt: ");
            for (var i = 0; i < devolver.Length; i++)
            {
                if (devolver[i] == sessions.Text)
                {
                    devolver = devolver.Where((source, index) => index != i).ToArray();
                }
            }

            if (cantidad == devolver.Length)
            {
                MessageBox.Show("There was nothing to erase");
                return;
            }

            string path;
            devolver = devolver.Where((source, index) => index != 0).ToArray();
            path = "bater\\sessions.txt";
            writeFile(path, devolver, "Session deleted: " + sessions.Text, false);

            InitializeSessions();

            path = @"bater\deleteSession.bat";
            var Arguments = new string[1];
            Arguments[0] = sessions.Text;
            ExecutarScript(path, Arguments);
        }

        // To execute .BAT
        private void ExecutarScript(string path, string[] Arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = path;
            psi.UseShellExecute = true;
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            for (var i = 0; i < Arguments.Length; i++)
            {
                psi.ArgumentList.Add(Arguments[i]);
            }
            Process.Start(psi);
        }

        // Write file
        private void writeFile(string path, string[] Arguments, string successMsg, bool appendOrNot = true)
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw;
                if (appendOrNot == true)
                {
                    sw = new StreamWriter(path, append: true);
                }
                else
                {
                    sw = new StreamWriter(path);
                }
                //Write a line of text
                for (var i = 0; i < Arguments.Length; i++)
                {
                    sw.WriteLine(Arguments[i]);
                }
                sw.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show("The file couldn't be written. Exception: " + err.Message);
            }
            finally
            {
                MessageBox.Show(successMsg);
            }
        }

        public string[] readFile(string path, string failureMsg)
        {

            string[] devolver = new string[1];
            devolver[0] = "";
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(path);

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                for (var i = 1; line != null; i++)
                {
                    //////////////////////////sessions.Items.AddRange(new object[] { line }); // to add sessions in the select box
                    //Read the next line
                    Array.Resize(ref devolver, devolver.Length + 1);
                    devolver[i] = line;
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(failureMsg + err.Message);
            }
            devolver = devolver.Where((source, index) => index != 0).ToArray();
            return devolver;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = @"bater\openFirefox.bat";
            var Arguments = new string[1];
            Arguments[0] = sessions.Text;
            ExecutarScript(path, Arguments);
        }
    }
}
