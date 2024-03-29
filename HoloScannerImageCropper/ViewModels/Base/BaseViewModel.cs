﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HoloScannerImageCropper
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
