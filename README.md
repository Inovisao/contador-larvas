# Contador de larvas de peixes

Software de detecção e contagem automática de larvas peixes usando machine learning.

### API:

##### Execução da API
1. Em primeiro lugar, é necessário colocar as redes treinadas nos caminhos ```data/models/<nome_da_rede>/<arquivo_da_rede>```, seguindo os caminhos previstos na classe ```ModelsPaths``` do arquivo ```src/predictor/counter.py```, sendo que as redes de mesma arquitetura (variando apenas o tamanho dela) são colocadas na mesma pasta. Obs: as redes YOLO e RT-DETR são apenas arquivos ```.pt```, enquanto a DETR e DEFORMABLE-DETR são pastas.
2. Após isso, basta apenas executar o arquivo ```src\api\app.py``` que a API estará pronta para o uso.

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
                "image": "ZGphc2lp...",
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
                "annotated_image": "XGphckG..."
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

### Dependências da API:
- conda create --name nome_do_seu_ambiente
- sudo apt install python3-pip (Atualizando o pip)
- pip install supervision==0.3.0
- pip install transformers (Versão 4.34.1)
- pip install pytorch-lightning (Versão 2.1.0)
- pip install timm
- pip install cython
- pip install pycocotools
- pip install scipy

### Instruções para utilização do aplicativo:
Após instalado, o aplicativo irá apresentar as seguintes opções:
1. Selecionar Fotos: essa opção levará para uma segunda tela onde o usuário poderá capturar novas fotos ou selecionar as já existentes em seu dispositivo. Tendo as imagens selecionadas, elas devem ser enviadas para o servidor através do botão "Enviar".
- O botão enviar fará a requisição, enviando a imagens para que o servidor faça a contagem. Após a resposta ser recebida, o usuário será direcionado à outra tela que exibirá os resultados, além de guardá-los em um banco de dados local.
- A tela de resultados exibirá as imagens anotadas juntamente com a quantidade de larvas presentes em cada uma delas. É possível visualizar todas as imagens enviadas passando para o lado.
2. Histórico: nesse botão leva para outra tela, aonde é possível visualizar o histórico de requisições feitas. Ao selecionar uma das opções, o usuário é direcionado para a tela de resultados.
3. Configurações: essa tela dá ao usuário a possibilidade de alterar a URL do servidor em que a API estará hospedada e possui um campo e um botão. No campo, deve ser colocado a URL desejada e confirmar esse link através do "Confirmar".
4. Parceiros: essa tela exibe os parceiros do projeto.

### Para geração do APK (feito no windows 10)
1. Criar um arquivo contendo a chave para geração do APK
- keytool -genkeypair -v -keystore fishlytics.keystore -alias fishlytics -keyalg RSA -keysize 2048 -validity 10000
- Deve-se prosseguir respondendo as perguntas que aparecerão (guarde a senha de armazenamento), são coisas pessoais e podem ser respondidas de qualquer forma.
2. Geração do APK (preencher utilzando o nome do arquivo, o alias utilizado e a senha definida no momento de criação do keystore)
- dotnet publish -f net7.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=fishlytics.keystore -p:AndroidSigningKeyAlias=fishlytics -p:AndroidSigningKeyPass=sua_senha -p:AndroidSigningStorePass=sua_senha
3. Resta apenas aguardar o término do processo. Os arquivos criados serão armazenados no caminho bin/Release/net7.0-android, é lá onde podemos encontrar o APK e outros arquivos caso haja o interesse de enviar para a playstore (loja de apps do google).