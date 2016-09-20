namespace P01_RIT_v2.UI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonInvertido = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.buttonColeccion = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.textBoxPrefijo = new MetroFramework.Controls.MetroTextBox();
            this.textBoxColeccion = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.textBoxInvertido = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.buttonIndexar = new MetroFramework.Controls.MetroTextBox.MetroTextButton();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.metroTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.tabPage1);
            this.metroTabControl1.Controls.Add(this.tabPage2);
            this.metroTabControl1.Location = new System.Drawing.Point(23, 63);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(611, 396);
            this.metroTabControl1.TabIndex = 17;
            this.metroTabControl1.UseSelectable = true;
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage1.Controls.Add(this.buttonInvertido);
            this.tabPage1.Controls.Add(this.buttonColeccion);
            this.tabPage1.Controls.Add(this.textBoxPrefijo);
            this.tabPage1.Controls.Add(this.textBoxColeccion);
            this.tabPage1.Controls.Add(this.metroLabel4);
            this.tabPage1.Controls.Add(this.textBoxInvertido);
            this.tabPage1.Controls.Add(this.metroLabel5);
            this.tabPage1.Controls.Add(this.buttonIndexar);
            this.tabPage1.Controls.Add(this.metroLabel6);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(603, 354);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Indexar";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(603, 354);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Consultas";
            // 
            // buttonInvertido
            // 
            this.buttonInvertido.Image = null;
            this.buttonInvertido.Location = new System.Drawing.Point(483, 211);
            this.buttonInvertido.Name = "buttonInvertido";
            this.buttonInvertido.Size = new System.Drawing.Size(51, 23);
            this.buttonInvertido.TabIndex = 26;
            this.buttonInvertido.Text = "...";
            this.buttonInvertido.Theme = MetroFramework.MetroThemeStyle.Light;
            this.buttonInvertido.UseSelectable = true;
            this.buttonInvertido.UseVisualStyleBackColor = true;
            // 
            // buttonColeccion
            // 
            this.buttonColeccion.Image = null;
            this.buttonColeccion.Location = new System.Drawing.Point(483, 149);
            this.buttonColeccion.Name = "buttonColeccion";
            this.buttonColeccion.Size = new System.Drawing.Size(51, 23);
            this.buttonColeccion.TabIndex = 25;
            this.buttonColeccion.Text = "...";
            this.buttonColeccion.Theme = MetroFramework.MetroThemeStyle.Light;
            this.buttonColeccion.UseSelectable = true;
            this.buttonColeccion.UseVisualStyleBackColor = true;
            this.buttonColeccion.Click += new System.EventHandler(this.buttonColeccion_Click);
            // 
            // textBoxPrefijo
            // 
            // 
            // 
            // 
            this.textBoxPrefijo.CustomButton.Image = null;
            this.textBoxPrefijo.CustomButton.Location = new System.Drawing.Point(398, 1);
            this.textBoxPrefijo.CustomButton.Name = "";
            this.textBoxPrefijo.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.textBoxPrefijo.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBoxPrefijo.CustomButton.TabIndex = 1;
            this.textBoxPrefijo.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBoxPrefijo.CustomButton.UseSelectable = true;
            this.textBoxPrefijo.CustomButton.Visible = false;
            this.textBoxPrefijo.Lines = new string[0];
            this.textBoxPrefijo.Location = new System.Drawing.Point(57, 93);
            this.textBoxPrefijo.MaxLength = 32767;
            this.textBoxPrefijo.Name = "textBoxPrefijo";
            this.textBoxPrefijo.PasswordChar = '\0';
            this.textBoxPrefijo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxPrefijo.SelectedText = "";
            this.textBoxPrefijo.SelectionLength = 0;
            this.textBoxPrefijo.SelectionStart = 0;
            this.textBoxPrefijo.Size = new System.Drawing.Size(420, 23);
            this.textBoxPrefijo.TabIndex = 17;
            this.textBoxPrefijo.UseSelectable = true;
            this.textBoxPrefijo.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBoxPrefijo.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // textBoxColeccion
            // 
            // 
            // 
            // 
            this.textBoxColeccion.CustomButton.Image = null;
            this.textBoxColeccion.CustomButton.Location = new System.Drawing.Point(398, 1);
            this.textBoxColeccion.CustomButton.Name = "";
            this.textBoxColeccion.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.textBoxColeccion.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBoxColeccion.CustomButton.TabIndex = 1;
            this.textBoxColeccion.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBoxColeccion.CustomButton.UseSelectable = true;
            this.textBoxColeccion.CustomButton.Visible = false;
            this.textBoxColeccion.Lines = new string[0];
            this.textBoxColeccion.Location = new System.Drawing.Point(57, 149);
            this.textBoxColeccion.MaxLength = 32767;
            this.textBoxColeccion.Name = "textBoxColeccion";
            this.textBoxColeccion.PasswordChar = '\0';
            this.textBoxColeccion.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxColeccion.SelectedText = "";
            this.textBoxColeccion.SelectionLength = 0;
            this.textBoxColeccion.SelectionStart = 0;
            this.textBoxColeccion.Size = new System.Drawing.Size(420, 23);
            this.textBoxColeccion.TabIndex = 18;
            this.textBoxColeccion.UseSelectable = true;
            this.textBoxColeccion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBoxColeccion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(57, 127);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(126, 19);
            this.metroLabel4.TabIndex = 23;
            this.metroLabel4.Text = "Ruta de la colección";
            // 
            // textBoxInvertido
            // 
            // 
            // 
            // 
            this.textBoxInvertido.CustomButton.Image = null;
            this.textBoxInvertido.CustomButton.Location = new System.Drawing.Point(398, 1);
            this.textBoxInvertido.CustomButton.Name = "";
            this.textBoxInvertido.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.textBoxInvertido.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBoxInvertido.CustomButton.TabIndex = 1;
            this.textBoxInvertido.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBoxInvertido.CustomButton.UseSelectable = true;
            this.textBoxInvertido.CustomButton.Visible = false;
            this.textBoxInvertido.Lines = new string[0];
            this.textBoxInvertido.Location = new System.Drawing.Point(57, 211);
            this.textBoxInvertido.MaxLength = 32767;
            this.textBoxInvertido.Name = "textBoxInvertido";
            this.textBoxInvertido.PasswordChar = '\0';
            this.textBoxInvertido.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxInvertido.SelectedText = "";
            this.textBoxInvertido.SelectionLength = 0;
            this.textBoxInvertido.SelectionStart = 0;
            this.textBoxInvertido.Size = new System.Drawing.Size(420, 23);
            this.textBoxInvertido.TabIndex = 19;
            this.textBoxInvertido.UseSelectable = true;
            this.textBoxInvertido.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBoxInvertido.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(57, 189);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(159, 19);
            this.metroLabel5.TabIndex = 22;
            this.metroLabel5.Text = "Ruta del archivo invertido";
            // 
            // buttonIndexar
            // 
            this.buttonIndexar.Image = null;
            this.buttonIndexar.Location = new System.Drawing.Point(57, 255);
            this.buttonIndexar.Name = "buttonIndexar";
            this.buttonIndexar.Size = new System.Drawing.Size(477, 29);
            this.buttonIndexar.TabIndex = 20;
            this.buttonIndexar.Text = "Indexar!";
            this.buttonIndexar.UseSelectable = true;
            this.buttonIndexar.UseVisualStyleBackColor = true;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(57, 71);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(149, 19);
            this.metroLabel6.TabIndex = 21;
            this.metroLabel6.Text = "Prefijo para los archivos";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 482);
            this.Controls.Add(this.metroTabControl1);
            this.Name = "MainWindow";
            this.Text = "Bievenido";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.metroTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private MetroFramework.Controls.MetroTextBox.MetroTextButton buttonInvertido;
        private MetroFramework.Controls.MetroTextBox.MetroTextButton buttonColeccion;
        private MetroFramework.Controls.MetroTextBox textBoxPrefijo;
        private MetroFramework.Controls.MetroTextBox textBoxColeccion;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox textBoxInvertido;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroTextBox.MetroTextButton buttonIndexar;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}