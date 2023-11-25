using AppContagem.Classes;
using AppContagem.Model;
using AppContagem.Pages;
using AppContagem.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppContagem.ViewModels
{
    public class HistoricoViewModel : clsNotificaBase
    {
        private ImageService _imageService;
        private ObservableCollection<ImageGroup> _imageGroup;
        public ObservableCollection<ImageGroup> ImageGroup { get { return _imageGroup; } set { _imageGroup = value; Notifica(); } }

        public HistoricoViewModel()
        {
            ImageGroup = new ObservableCollection<ImageGroup>();
            _imageService = new ImageService();
            new Action(async () => await GetFileGroups())();
        }

        public async void Ver_Fotos(ImageGroup imageGroup)
        {
            await _imageService.Init();
            var images = (await _imageService.GetImages()).Where(c => c.group_id == imageGroup.groupId).ToList();

            var imagesDTO = images.Select(im => new ImageDTO
            {
                _id = im.nome,
                confiance = im.confiance,
                total_count = im.total_count,
                path = im.path
            }).ToList();

            await App.Current.MainPage.Navigation.PushAsync(new Resultados(imagesDTO));
        }

        public async Task GetFileGroups()
        {
            await _imageService.Init();
            var images = (await _imageService.GetImages()).GroupBy(c => c.group_id).Select(c => c.FirstOrDefault()).ToList();

            foreach (var img in images)
            {
                ImageGroup.Add(new ImageGroup
                {
                    groupId = img.group_id,
                    data = new DateTime(img.group_id.Value)
                });
            }
        }
    }
}
