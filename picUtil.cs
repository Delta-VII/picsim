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



    class PicUtil
    {
        private Pic _uc = new Pic();
        private string[] _program;
        public TransferGuiToSim Tgui2Sim;
        public TransferSimToGui Tsim2Gui;

        public event EventHandler<NewDataReceiedEventArgs> EvtNewDataReceived;
        public event EventHandler<UpdateRegistersEventArgs> EvtUpdateRegisters;

        public PicUtil()
        {
            _uc.EvtUpdateRegisters += OnEvtUpdateRegisters;
            Tgui2Sim = new TransferGuiToSim();
            Tsim2Gui = new TransferSimToGui();
        }

        public async void Run()
        {
            await _uc.Step();
        }

        public void InitSimulator(List<string> s)
        {
            _uc.SetProgramMemory(s.ToArray());
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

        public TransferSimToGui Tsim2Gui1
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
            this.Tsim2Gui1.Ram1 = _uc.Ram;
            this.Tsim2Gui1.Laufzeit = _uc.ProgrammLaufzeit;
            this.Tsim2Gui1.Stack1 = _uc.Stack;
            this.Tsim2Gui1.Stackpointer1 = _uc.Stackpointer;
            EvtNewDataReceived?.Invoke(this, new NewDataReceiedEventArgs(true));
        }
    }

}