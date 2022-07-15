using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Try
{
    class controller
    {
        private Pic uc = new Pic();
        private string[] Program;
        private TransferGuiToSim Tgui2Sim;
        private TransferSimToGUI Tsim2Gui;

        public TransferGuiToSim GetTgui2Sim(TransferGuiToSim guiToSim)
        {
            return guiToSim;
        }

        public async void test()
        {
            await uc.Step(0); 
        }
        
        public void InitSimulator()
        {
            
        }

        

    }
    
}