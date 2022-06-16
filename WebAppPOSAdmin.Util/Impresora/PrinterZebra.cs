using System.ComponentModel;

namespace WebAppPOSAdmin.Util.Impresora
{
    public class PrinterZebra : Component
    {
        private IContainer components;

        public PrinterZebra()
        {
            InitializeComponent();
        }

        public PrinterZebra(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
        }
    }
}
