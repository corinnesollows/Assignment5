//Copyright (c) 2002, Art Gittleman
//This example is provided WITHOUT ANY WARRANTY 
//either expressed or implied.

/* Illustrates some menu and dialog features.
 * Corinne Jones-hoyland & Melanie Damilig
 */

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
public class MenuDialog : Form {
    private long totalFileLength;
    TextBox text = new TextBox();

    // Create the form
    public MenuDialog() {
        Size = new Size(600,300);
        Text = "Menus and Dialogs";
        Button clearBox = new Button();
        clearBox.Text = "Clear Box";

        text.Size = new Size(450,120);
        text.Multiline = true;
        text.ScrollBars = ScrollBars.Both;
        text.WordWrap = true;
        text.Location = new Point(20,20);
        text.Text += "";
        clearBox.Location = new Point(20, 145);
        Controls.Add(clearBox); 

        // Configure file menu
        MenuItem fileMenu = new MenuItem("File");
        MenuItem open = new MenuItem("Open");
        MenuItem readbytes = new MenuItem("Read Bytes");
        MenuItem writebinary = new MenuItem("Write Binary");
        MenuItem readbinary = new MenuItem("Read Binary");
        open.Shortcut = Shortcut.CtrlO;
        MenuItem save = new MenuItem("SaveTextBox");
        save.Shortcut = Shortcut.CtrlS;
        fileMenu.MenuItems.Add(open);
        fileMenu.MenuItems.Add(readbytes);
        fileMenu.MenuItems.Add(writebinary);
        fileMenu.MenuItems.Add(readbinary);
        fileMenu.MenuItems.Add(save);

        // Configure feedback menu 
        MenuItem feedbackMenu = new MenuItem("Feedback");
        MenuItem message = new MenuItem("Message");
        message.Shortcut = Shortcut.CtrlM;
        feedbackMenu.MenuItems.Add(message); 

        // Configure format menu
        MenuItem formatMenu = new MenuItem("Format");
        MenuItem font = new MenuItem("Font");
        font.Shortcut = Shortcut.CtrlF;
        formatMenu.MenuItems.Add(font);

        // Configure args menu
        MenuItem argsMenu = new MenuItem("Args");
        MenuItem showArgs = new MenuItem("Args");
        argsMenu.MenuItems.Add(showArgs);

        // Configure programs menu
        MenuItem programs = new MenuItem("Programs");
        MenuItem stringtonumber = new MenuItem("String to Number");
        MenuItem fileproperties = new MenuItem("File Properties");
        programs.MenuItems.Add(stringtonumber);
        programs.MenuItems.Add(fileproperties); 
      
        // Configure main menu
        MainMenu bar = new MainMenu();
        Menu = bar;
        bar.MenuItems.Add(fileMenu);
        bar.MenuItems.Add(feedbackMenu);
        bar.MenuItems.Add(formatMenu);
        bar.MenuItems.Add(argsMenu);
        bar.MenuItems.Add(programs);

        // Add control to form
        Controls.Add(text);

        // Register event handlers  
        open.Click += new EventHandler(Open_Click);
        readbytes.Click += new EventHandler(read_Bytes_Click);
        writebinary.Click += new EventHandler(Write_Binary_Click);
        readbinary.Click += new EventHandler(Read_Binary_Click);
        save.Click += new EventHandler(Save_Click);
        message.Click += new EventHandler(Message_Click);
        font.Click += new EventHandler(Font_Click);
        showArgs.Click += new EventHandler(Args_Click);
        stringtonumber.Click += new EventHandler(String_to_Number_Click);
        fileproperties.Click += new EventHandler(File_Properties_Click);
        clearBox.Click += new EventHandler(ClearBox_Click);
    }

    protected void String_to_Number_Click(Object sender, EventArgs e) {
        try {
            int i = int.Parse("435");
            Console.WriteLine("i = {0}", i);
            int j = int.Parse("45.2");
            Console.WriteLine("j = {0}", j);
        }
        catch (FormatException er) {
            Console.WriteLine(er);
            double d = double.Parse("3.14");
            Console.WriteLine("d = {0}", d);
        }
        string message = "This item is not ready yet!";
        string caption = "Under Construction";
	    MessageBox.Show(message, caption);
    }

    protected void File_Properties_Click(Object sender, EventArgs e) {
        OpenFileDialog o = new OpenFileDialog();
        if (o.ShowDialog() == DialogResult.OK) {
            String name = o.FileName;
            FileInfo f = new FileInfo(name);
            text.Text += "Name: " + f.Name + "\r\n";
            text.Text += "Full name: " + f.FullName + "\r\n";
            text.Text += "Creation time: " + f.CreationTime + "\r\n";
            text.Text += "Last access time: " + f.LastAccessTime + "\r\n";
            text.Text += "Length: " + f.Length + "\r\n";
            text.Text += "Parent directory: " + f.DirectoryName + "\r\n";
        }
    }

    protected void Args_Click(Object sender, EventArgs e) {
        string[]args = Environment.GetCommandLineArgs();
        if (args.Length == 0)
            text.Text = "No Arguments Passed";
        else {
            text.Text = "";
            for (int i = 0; i < args.Length; i++)
                text.Text += args[i];
        }
    }

    protected void read_Bytes_Click(Object sender, EventArgs e) {
        OpenFileDialog o = new OpenFileDialog();
        if (o.ShowDialog() == DialogResult.OK) {
            Stream file = o.OpenFile();
            StreamReader reader = new StreamReader(file);
            BinaryReader input;
            long fileLength = file.Length;
            input = new BinaryReader(file);
            text.Text += "\r\nRead and list file values byte by byte: ";
            long num = 0;
            byte b;
            for (long i = 0; i < fileLength || i == 1000; i++) {
                b = input.ReadByte();
                text.Text += b;
                num++;
            }
            char[] data = new char[file.Length];
            reader.ReadBlock(data, 0, (int)file.Length);
            reader.Close();
            text.Text += "\r\nByte Count = " + num;
        }
    }

    // Handle WriteBinary menu item
    protected void Write_Binary_Click(Object sender, EventArgs e) {
        SaveFileDialog s = new SaveFileDialog();
        if (s.ShowDialog() == DialogResult.OK) {
            String name = s.FileName;
            BinaryWriter output = new BinaryWriter(new FileStream(name, FileMode.Create));
            for (int i = 0; i < 10; i++)
                output.Write(i);
            for (double d = 0.0; d < 10.0; d++)
                output.Write(d);
            output.Close();
        }
    }

    // Handle ReadBinary menu item
    protected void Read_Binary_Click(Object sender, EventArgs e) {
        OpenFileDialog o = new OpenFileDialog();
        if (o.ShowDialog() == DialogResult.OK) {
            String name = o.FileName;
            BinaryReader input = new BinaryReader(new FileStream(name, FileMode.Open));
            text.Text += "\r\nReading 10 Int32 values from file: " + name + "\r\n";
            for (int i = 0; i < 10; i++)
                text.Text += input.ReadInt32() + " ";
            text.Text += "\r\n";
            text.Text += "\r\nReading 10 Double values from file: " + name + "\r\n";
            for (int i = 0; i < 10; i++)
                text.Text += input.ReadDouble().ToString("0.0") + " ";
            input.Close();
        }
    }

    protected void ClearBox_Click(Object sender, EventArgs e) {
      text.Text = "";
    }
  
    // Handle open menu item
    protected void Open_Click(Object sender, EventArgs e) {
        OpenFileDialog o = new OpenFileDialog();
        if(o.ShowDialog() == DialogResult.OK) {
            Stream file = o.OpenFile();
            StreamReader reader = new StreamReader(file);
            char[] data = new char[file.Length];
            reader.ReadBlock(data, 0, (int)file.Length);
            long fileLength = file.Length;
            totalFileLength += file.Length;
            text.Text += "\r\nFile length = " + fileLength + "\r\n";
            text.Text += new String(data);
            text.Text += "\r\nTotal File length = " + totalFileLength;
            reader.Close();
        }
    }

    // Handle save menu item
    protected void Save_Click(Object sender, EventArgs e) {
        SaveFileDialog s = new SaveFileDialog();
        if(s.ShowDialog() == DialogResult.OK) {
            StreamWriter writer = new StreamWriter(s.OpenFile());
            writer.Write(text.Text);
            writer.Close();
        }
    }

    // Handle message menu
    protected void Message_Click(Object sender, EventArgs e) {
        MessageBox.Show("You clicked the Message menu", "My message");
    }

    // Handle font menu
    protected void Font_Click(Object sender, EventArgs e) {
        FontDialog f = new FontDialog();
        if(f.ShowDialog() == DialogResult.OK) 
            text.Font = f.Font;
    }
    
    [STAThread]
    public static void Main() {
        Application.Run(new MenuDialog());
    }
}
