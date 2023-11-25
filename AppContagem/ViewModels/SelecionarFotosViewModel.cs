using Acr.UserDialogs;
using AppContagem.Classes;
using AppContagem.Pages;
using AppContagem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContagem.ViewModels
{
    public class SelecionarFotosViewModel : clsNotificaBase
    {
        private List<FileResult> files;
        private ImageService _imageService;
        private ObservableCollection<string> filesListView;
        public List<FileResult> Files { get { return files; } set { files = value; Notifica(); } }
        public ObservableCollection<string> FilesListView { get { return filesListView; } set { filesListView = value; Notifica(); } }

        public SelecionarFotosViewModel()
        {
            _imageService = new ImageService();
            filesListView = new ObservableCollection<string>();
            Files = new List<FileResult>();
        }

        public Command enviaParaApiCommand
        {
            get
            {
                return new Command(() => EnviaParaAPI());
            }
        }

        public async void SelecionarFoto()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status == PermissionStatus.Denied)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            if (status == PermissionStatus.Granted)
            {
                var options = new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                };
                var files = (await FilePicker.PickMultipleAsync()).ToList();

                if (files != null && files.Count() > 0)
                {
                    files = files.Where(c => c.ContentType.Contains("image")).ToList();
                    Files.AddRange(files);
                    files.ForEach(c => FilesListView.Add(c.FileName));
                }
            }
        }

        public async void CapturarFoto()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Denied)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status == PermissionStatus.Granted)
            {
                var file = await MediaPicker.CapturePhotoAsync();

                if (file != null)
                {
                    Files.Add(file);
                    FilesListView.Add(file.FileName);
                }
            }
        }

        public async void EnviaParaAPI()
        {
            var groupId = DateTime.Now.Ticks;
            var images = new List<ImageSendRequest>();
            await _imageService.Init();

            if (Files == null || Files.Count <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Negado", "Selecione uma ou mais imagens para prosseguir.", "OK");
                return;
            }

            Files.ForEach(image =>
            {
                var s = System.IO.File.ReadAllBytes(image.FullPath);
                images.Add(new ImageSendRequest
                {
                    _id = image.FileName,
                    image = Convert.ToBase64String(s),
                    grid_scale = 0.3,
                    confiance = 0.5,
                    return_image = true
                });
            });

            UserDialogs.Instance.ShowLoading("Aguarde...");
            var result = await new ContagemService().Post(images);
            UserDialogs.Instance.HideLoading();

            if (result == null)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Houve um erro ao tentar realizar a operação. Tente novamente mais tarde." , "OK");
                return;
            }

            result.results.ForEach(async image =>
            {
                var filePath = Path.Combine(FileSystem.CacheDirectory, image._id);
                var stream = new StreamContent(new MemoryStream(Convert.FromBase64String(image.annotated_image)));

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await stream.CopyToAsync(fileStream);
                    fileStream.Flush();
                }
                image.path = filePath;

                var imagedb = image.ToImage(groupId);
                await _imageService.AddImage(imagedb);
            });

            await App.Current.MainPage.Navigation.PushAsync(new Resultados(result.results));
        }
    }
}

