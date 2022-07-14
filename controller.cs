using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace picsim
{
    class controller
    {
        private Pic uc = new Pic();
        private static Timer systick;
        private string[] Program;

        public async void test()
        {
            await uc.Step(0); 
        }
        
        public void InitSimulator()
        {
            
        }



    }
    
}