using HoloScannerImageCropper.ImageProcessing;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HoloScannerImageCropper
{
    public class MainViewModel : BaseViewModel
    {
        #region Commands
        public ICommand LoadImagesFromFolderCommand { get; set; }
        public ICommand CropImagesCommand { get; set; }
        #endregion

        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                _selectedPath = value;
                ShowInvalidPathText = false;
            }
        }

        public ObservableCollection<string> FileNames { get; set; }
        public List<string> FilePaths { get; set; }

        public bool ShowInvalidPathText { get; set; } = false;

        public bool ValidPathSelected { get; set; } = false;

        public MainViewModel()
        {
            FileNames = new ObservableCollection<string>();

            LoadImagesFromFolderCommand = new DelegateCommand(LoadImagesFromFolder);
            CropImagesCommand = new DelegateCommand(CropImages);
        }

        private void LoadImagesFromFolder(object obj)
        {
            if (!Directory.Exists(SelectedPath))
            {
                ShowInvalidPathText = true;
                ValidPathSelected = false;
                return;
            }
            string[] paths = Directory.GetFiles(SelectedPath);

            FilePaths = paths.Where(_ => _.ToLower().EndsWith(".png")).ToList();
            paths = FilePaths.Select(_ => Path.GetFileNameWithoutExtension(_)).ToArray();

            foreach (var path in paths)
            {
                FileNames.Add(path);
            }

            ValidPathSelected = true;
        }

        private void CropImages(object obj)
        {
            string destination = $"{ SelectedPath }Cropped";
            Directory.CreateDirectory(destination);
            Cropper.CropMax(FilePaths, destination);

            MessageBox.Show("Images where cropped!", "Success!");
        }
    }
}
