using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValidateTextBoxComponent
{
    public enum eTipo
    {
        Numerico,
        Textual
    }

    [
        DefaultProperty("Tipo"),
        DefaultEvent("TxtChanged")
    ]
    public partial class ValidateTextBox : UserControl
    {
        bool colorRojo = true;

        private eTipo tipo;

        [Category("Design")]
        [Description("Tipo de caracteres del texto del TextBox")]
        public eTipo Tipo{
            set
            {
                if(Enum.IsDefined(typeof(eTipo), value))
                {
                    tipo = value;
                }
                else
                {
                    throw new InvalidEnumArgumentException();
                }
            }
            get
            {
                return tipo;
            }
        }

        [Category("Appearance")]
        [Description("Texto asociado al TextBox del control")]
        public String Texto
        {
            set
            {
                txt.Text = value;                

                if (Tipo == eTipo.Numerico)
                {
                    try
                    {
                        colorRojo = false;
                        Convert.ToInt32(txt.Text.Trim());
                    }
                    catch (FormatException)
                    {
                        colorRojo = true;
                    }
                }
                else
                {
                    string texto = txt.Text.ToLower();
                    colorRojo = false;
                    for (int i = 0; i < texto.Length; i++)
                    {
                        //if(Multilinea && texto[i] == '\r' && texto[i+1] != '\n')
                        //{
                        //    colorRojo = true;
                        //}else
                        if ((texto[i] < 'a' || texto[i] > 'z') && texto[i] != ' ' && texto[i] != 'ñ')
                        {                            
                            colorRojo = true;
                        }
                    }
                }
                Refresh();
            }
            get
            {
                return txt.Text;
            }
        }

        [Category("Behavior")]
        [Description("Controla si el texto del TextBox puede ocupar más de una línea")]
        public bool Multilinea
        {
            set
            {
                txt.Multiline = value;
            }
            get
            {
                return txt.Multiline;
            }
        }

        [Category("El texto de Textbox cambió")]
        [Description("Se lanza cuando el texto de la Textbox cambia")]
        public event System.EventHandler TxtChanged;

        public ValidateTextBox()
        {
            InitializeComponent();
            tipo = eTipo.Numerico;
            colorRojo = true;
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            Texto = txt.Text;
            this.TxtChanged?.Invoke(sender, e);          //Acceso ao evento desde o compoñente  
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            txt.Location = new Point(10, 10);
            this.Height = txt.Height + 20;
            txt.Width = this.Width - 20;

            Pen p = new Pen(colorRojo ? Color.Red : Color.Green);
            p.Width = 3;
            e.Graphics.DrawRectangle(p, new Rectangle(5, 5, this.Width - 10, this.Height - 10));

            e.Dispose();
        }
    }
}
