using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Try
{
    public class NewDataReceiedEventArgs : EventArgs
    {
        private bool Test { get; set; }
        public NewDataReceiedEventArgs(bool test)
        {
            Test = test;
        }
    }

    public class UpdateRegistersEventArgs : EventArgs
    {
        private int Test { get; set; }
        public UpdateRegistersEventArgs(int test)
        {
            Test = test;
        }
    }



    class picUtil
    {
        private Pic uc = new Pic();
        private string[] Program;
        private TransferGuiToSim Tgui2Sim;
        private TransferSimToGUI Tsim2Gui;

        public event EventHandler<NewDataReceiedEventArgs> EvtNewDataReceived;

        public picUtil()
        {
            uc.EvtUpdateRegisters += OnEvtUpdateRegisters;
        }

        public async void Run()
        {
            await uc.Step();
        }

        public void InitSimulator(List<string> s)
        {
            uc.SetProgramMemory(s.ToArray());
        }

        public TransferGuiToSim Tgui2Sim1
        {
            //get => Tgui2Sim;
            //set => Tgui2Sim = value ?? throw new ArgumentNullException(nameof(value));
            get
            {
                return Tgui2Sim;
            }
            set
            {
                Tgui2Sim = value ?? throw new ArgumentNullException(nameof(value));
                //EvtNewDataReceived?.Invoke(this, new EventArgs.Empty);
            }
        }

        public TransferSimToGUI Tsim2Gui1
        {
            //get => Tsim2Gui;
            //set => Tsim2Gui = value ?? throw new ArgumentNullException(nameof(value));
            get
            {
                return Tsim2Gui;
            }
            set
            {
                Tsim2Gui = value ?? throw new ArgumentNullException(nameof(value));
                EvtNewDataReceived?.Invoke(this, new NewDataReceiedEventArgs(false));
            }
        }

        private void OnEvtUpdateRegisters(object sender, UpdateRegistersEventArgs e)
        {
            this.Tsim2Gui1.Ram1 = uc._ram;
            this.Tsim2Gui1.Laufzeit = uc._laufzeit;
            this.Tsim2Gui1.Stack1 = uc._stack;
            this.Tsim2Gui1.Stackpointer1 = uc._stackpointer;
            EvtNewDataReceived?.Invoke(this, new NewDataReceiedEventArgs(true));
        }
    }

}