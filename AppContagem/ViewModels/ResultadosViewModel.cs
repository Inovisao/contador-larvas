using AppContagem.Classes;
using AppContagem.Services;
using AppContagem.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppContagem.ViewModels
{
    public class ResultadosViewModel : clsNotificaBase
    {
        private ObservableCollection<ImageDTO> images;
        private ImageService _imageService;
        public ObservableCollection<ImageDTO> Images { get { return images; } set { images = value; Notifica(); } }
        public ResultadosViewModel(List<ImageDTO> imagesP)
        {
            _imageService = new ImageService();
            new Action(async () => { await PopulaResultados(imagesP); })();
        }

        public async Task PopulaResultados(List<ImageDTO> imagesP)
        {
            var obsimages = new ObservableCollection<ImageDTO>();
            
            foreach (var image in imagesP)
            {
                obsimages.Add(image);
            }

            Images = obsimages;
        }
    }
}
