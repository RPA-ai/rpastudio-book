﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutofacIoCSample.Services
{
    public class SingleInstanceClass
    {
        private int num = 0;

        public void Show()
        {
            MessageBox.Show((++num).ToString());
        }
    }
}
