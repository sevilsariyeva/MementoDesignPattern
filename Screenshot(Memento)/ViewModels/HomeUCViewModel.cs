using Screenshot_Memento_.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;

namespace Screenshot_Memento_.ViewModels
{
    public class HomeUCViewModel : BaseViewModel
    {
        public RelayCommand TakeScreenClickCommand { get; set; }
        public RelayCommand BackClickCommand { get; set; }
        public RelayCommand NextClickCommand { get; set; }
        List<string> Images = new List<string>();
        private List<IImageMemento> _imageMementos;
        private int _currentIndex;
       
        private string currentImage;

        public string CurrentImage
        {
            get { return currentImage; }
            set { currentImage = value; OnPropertyChanged(); }
        }
        public static int currentIndex=-1;

        
        public interface IImageMemento
        {
            string ImagePath { get; }
        }
        public class ImageMemento : IImageMemento
        {
            public string ImagePath { get; private set; }

            public ImageMemento(string imagePath)
            {
                ImagePath = imagePath;
            }
        }
        private void TakeScreen()
        {
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;
            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Default);
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(Application.Current.MainWindow);
                drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), new Point(width, height)));
            }
            bitmap.Render(drawingVisual);

            // Save the screenshot to the "Images" folder in the project
            string programFolderPath = AppDomain.CurrentDomain.BaseDirectory;

            string currentDirectory = Environment.CurrentDirectory;
            string debugremoved = Path.GetDirectoryName(currentDirectory);
            string binremoved = Path.GetDirectoryName(debugremoved);
            string folderPath = binremoved + "\\Images";
            string fileName = $"\\screenshot_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.png";
            string fullPath = folderPath + fileName;
            Images.Add(fullPath);
            Directory.CreateDirectory("Images");
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fileStream);
            }

            _imageMementos.Add(new ImageMemento(fullPath));
            _currentIndex++;
            if (Images.Count != 0)
            {
                CurrentImage = Images.ElementAt(Images.Count - 1);
            }
        }

        private void Back()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                CurrentImage = _imageMementos[_currentIndex].ImagePath;
            }
        }

        private void Next()
        {
            if (_currentIndex < _imageMementos.Count - 1)
            {
                _currentIndex++;
                CurrentImage = _imageMementos[_currentIndex].ImagePath;
            }
        }
        
        public HomeUCViewModel()
        {
            _imageMementos = new List<IImageMemento>();
            TakeScreenClickCommand = new RelayCommand((obj) =>
            {
                TakeScreen();
            });
            BackClickCommand = new RelayCommand((obj) =>
            {
                Back();

            });
            NextClickCommand = new RelayCommand((obj) =>
            {
                Next();
            });
        }
    }
}
