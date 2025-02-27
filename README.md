# Contador de larvas de peixes

Software de detecção e contagem automática de larvas peixes usando machine learning.

### Dependências da API:
- conda create --name contador-larvas python=3.11
- conda activate contador-larvas
- conda install pip
- conda install pytorch torchvision torchaudio pytorch-cuda=12.1 -c pytorch -c nvidia
- pip install numpy urllib3 flask requests opencv-python Pillow scipy shapely
- pip install transformers
- pip install ultralytics

### ngrock
- A API precisa ser levantada usando o ngrock
- seguir a [documentação oficial do ngrok](https://ngrok.com/docs/getting-started/)
- a porta usada pelo ngrock é definida no final do arquivo ```src\api\app.py``` na linha:
```python
if __name__ == '__main__':
    application.run(host='0.0.0.0', port=<PORTA_DO_NGROCK>, debug=False)
```

Para permitir comunicação entre API e app, utilize o seguinte comando:
```ngrok http 3000```


### API:

##### Execução da API
1. Em primeiro lugar, é necessário colocar as redes treinadas nos caminhos ```data/models/<nome_da_rede>/<arquivo_da_rede>```, seguindo os caminhos previstos na classe ```ModelsPaths``` do arquivo ```src/predictor/counter.py```, sendo que as redes de mesma arquitetura (variando apenas o tamanho dela) são colocadas na mesma pasta. Obs: as redes YOLO e RT-DETR são apenas arquivos ```.pt```, enquanto a DETR e DEFORMABLE-DETR são pastas.
    - Voce pode encontrar os modelos originalmente treinados para este projeto na Ada, no caminho:
    ```GHD/modelosDoInovisao/larvasDePeixes/data-contador-larvas.rar```
2. Após isso, basta apenas executar o arquivo ```src\api\app.py``` que a API estará pronta para o uso.
    - Caso queira utilizar as imagens do dataset de larvas usados originalmente neste projeto, por favor siga o seguinte caminho na Ada:
    ```GHD/bancosDeImagens/Peixes/Larvas/ContagemLarvas/train```

##### Uso da API:
1. Rotas da API:
    * ```/```: Verifica se a API está online
    * ```/set-params```: Modifica os parâmetros da classe do contador de larvas ```CounterModel```, por enquanto o único parâmetro modificável é o nome da rede. Ex:
        * POST &rarr; 
        ```json
        {"model_name": "rtdetr-x"}
        ``` 
        ```json
        {"model_name": "yolov8n"}
        ```
    * ```/get-params```: Retorna os parâmetros da classe ```CounterModel```.
    *  ```/contador-alevinos```: Rota para a contagem das larvas de peixe, recebe os seguintes argumentos em uma lista de json:
    * POST &rarr; ```/contador-alevinos```; 
    ```json
        [
            {
                "_id": "id_da_imagem",
                "image": "<string_da_imagem_em_base_64>",
                "grid_scale": 0.3,
                "confiance": 0.5,  
                "return_image": true,
            },
            {"args_2": "..."},
            {"args_3": "..."}, 
            {"args_n": "..."} 
        ]
    ```
    * Retorno da rota ```/contador-alevinos```:
    ```json
    {
        "results": [
            {
                "_id": "id_da_imagem",
                "grid_scale": 0.3,
                "confiance": 0.5,           
                "total_count": 104,          
                "grid_results": [              
                    {"grid_xyxy": [1,2,3,4], "grid_index": [1,2]}, {"...": "..."}
                ],
                "annotated_image": "<string_da_imagem_anotada_em_base_64>"
            },
            {"res_2": "..."}, 
            {"res_3": "..."}, 
            {"res_n": "..."} 
        ]
    }
    ```


2. O arquivo ```src\api\test_api.py``` faz alguns testes automáticos na API, tanto na rota ```/contador-alevinos``` quanto nas rotas ```/set-params``` e ```/get-params```. Tem-se a opção de chamar os métodos individuais de teste, ou chamar o método ```.test_all()```, que fará um teste generalizado automático. Para fazer testes visuais e ver as anotações das imagens, use os métodos ```.test_visual_one_image()``` ou ```.test_visual_many_images()```



### Instrução de como o contador de larvas foi feito:
1. Treino das redes:
- As redes foram treinadas com cortes de 640x640 pixels de imagens de larvas de peixes. Os arquivos para o treinos são individuais para cada rede e estão em ```src/models/<nome_da_rede>/train_<nome_da_rede>```
2. Ajuste dos parâmetros de inferência das redes:
- Depois de treinar as redes, cada modelo teve os seus 3 parâmetros de inferência ```(resize_scale, grid_scale, confiance)``` permutados dentro de uma faixa para obter a melhor combinação desses 3 parâmetros com base nas métricas MAE, MAPE e RMSE. Esse passo é feito no arquivo ```utils/metrics/metrics_permutation.py```. Nele tem a classe ```ArgsPermutator```, na qual o método ```.add()``` adiciona uma nova rede com uma faixa de parâmetros para ser permutados e testados um a um dentro do dataset escolhido. No fim é gerado um arquivo com os melhores parâmetros na pasta ```resuls/params_comparison/```, e deles se define o melhor parâmetro usado para cada rede.
3. Para informações detalhadas, consultar o artigo ```Contagem e detecçãao em imagens com alta densidade de larvas de peixes``` no repositório de artigos do INOVISÃO


### Instruções para utilização do aplicativo:
Após instalado, o aplicativo irá apresentar as seguintes opções:
1. Selecionar Fotos: essa opção levará para uma segunda tela onde o usuário poderá capturar novas fotos ou selecionar as já existentes em seu dispositivo. Tendo as imagens selecionadas, elas devem ser enviadas para o servidor através do botão "Enviar".
- O botão enviar fará a requisição, enviando a imagens para que o servidor faça a contagem. Após a resposta ser recebida, o usuário será direcionado à outra tela que exibirá os resultados, além de guardá-los em um banco de dados local.
- A tela de resultados exibirá as imagens anotadas juntamente com a quantidade de larvas presentes em cada uma delas. É possível visualizar todas as imagens enviadas passando para o lado.
2. Histórico: nesse botão leva para outra tela, aonde é possível visualizar o histórico de requisições feitas. Ao selecionar uma das opções, o usuário é direcionado para a tela de resultados.
3. Configurações: essa tela dá ao usuário a possibilidade de alterar a URL do servidor em que a API estará hospedada e possui um campo e um botão. No campo, deve ser colocado a URL desejada do ngrok e confirmar esse link através do "Confirmar".
4. Parceiros: essa tela exibe os parceiros do projeto.

### Para geração do APK (feito no windows 10)
1. Criar um arquivo contendo a chave para geração do APK
- keytool -genkeypair -v -keystore fishlytics.keystore -alias fishlytics -keyalg RSA -keysize 2048 -validity 10000
- Deve-se prosseguir respondendo as perguntas que aparecerão (guarde a senha de armazenamento), são coisas pessoais e podem ser respondidas de qualquer forma.
2. Geração do APK (preencher utilzando o nome do arquivo, o alias utilizado e a senha definida no momento de criação do keystore)
- dotnet publish -f net7.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=fishlytics.keystore -p:AndroidSigningKeyAlias=fishlytics -p:AndroidSigningKeyPass=sua_senha -p:AndroidSigningStorePass=sua_senha
3. Resta apenas aguardar o término do processo. Os arquivos criados serão armazenados no caminho bin/Release/net7.0-android, é lá onde podemos encontrar o APK e outros arquivos caso haja o interesse de enviar para a playstore (loja de apps do google).

### Instrucoes para rodar o app (testado no Ubunto 22.04.3 LTS com a versao 7.0.14 do dotnet)
1. Necessario a utilizacao do VSCODE (baixado pela loja de apps do ubunto)
2. Execute o seguinte comando para baixar os scripts de instalacao: wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
3. Para instalar os scripts, primeiro rode os comandos: 
- chmod +x dotnet-install.sh
- ./dotnet-install.sh --channel 7.0
4. Agora, sera necessario direcionar a variavel de ambiente do dotnet para o local de instalacao, para cada computador sera diferente. Esse local sera exibido apos a execucao do ultimo comando.
- Acesse o arquivo de configuracao com o comando (tambem sera necessario identificar o local deste arquivo, por padrao, ele pode ser encontrado na base do linux): open .bashrc
- O ultimo comando abrira um .txt para a edicao das variaveis de ambiente, nele, insira os seguintes parametros: 
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
- Apos isso, o arquivo deve ser salvo e a instalacao pode ser verificada no terminal com o comando "dotnet --info" (antes disso, digite no terminal "source .bashrc")
5. Para a instalacao do dotnet maui, execute o comando: dotnet workload install maui-android
6. Instalacao do Open JDK
- Execute os seguintes comandos para baixar os pacotes:
ubuntu_release=`lsb_release -rs`
wget https://packages.microsoft.com/config/ubuntu/${ubuntu_release}/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
- E esses para a instalacao:
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install msopenjdk-11
7. Faca a instalacao do android studio (encontrado na loja de apps do ubunto)
8. Instalar as extensoes ".NET MAUI", "C#" e ".NET Install Tool" no vscode
9. Configurar o Android SDK no vscode
- Um aviso sera exibido dizendo que o Android SDK nao foi encontrado, entao, clique em "configure", "Android SDK..." e selecione a pasta aonde o SDK esta instalado, por padrao, ele sera encontrado aqui: "/home/seu_usuario/Android/Sdk"
10. Apos isso, e necessario fazer a conexao com o celular (utilizei USB, e necessario ativar o modo desenvolvedor e o USB debugging) e selecionar o seu aparelho como dispositivo de saida
11. Ao lado esquerdo do vscode, clique em "run and debug" e espere ate que o app seja compilado. Um pop-up ira aparecer em seu celular pedindo permissao para a instalacao do aplicativo, entao, clique em aceitar.
12. Mais informacoes podem ser encontradas nesse video: https://www.youtube.com/watch?v=4D2vUYUIqFU




