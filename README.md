# ServerWorkers
Website configurado com Web API e interface para consultar e atualizar o banco de dados de funcionários.
O projeto foi desenvolvido a partir de outro projeto de automação comercial com BD em SQL com aproximadamente 150 tabelas. Optei por fazer desta forma, pois é a forma como vinha programando em um projeto pessoal.
No projeto ServerWorkers, foi criado o Web API com acesso pela url http://localhost:7607/api/Workers
O projeto tem o BD DBSystem que contem tabelas auxiliares que fornecem ao programa, informações necessárias para enviar ou receber informações do client, (tags, placeholders, panels, templates, descrições de campos, helps e etc),  algumas classes, jquery, css e etc.
Na interface web não consegui comunicar com o serviço Web API, pois apresentou problemas ao usar funções assincronas, (HttpClient c/ os metodos GetAsync, PutAsync, PostAsync e etc). Pelos meus testes só está funcionando dentro do "Page_Load". Fiz várias tentativas c/ diversas configurações, porém sem sucesso. Estou fazendo o acesso direto ao BD.
Na interface com Windows Form (ClientWorkers), funciona normalmente. Uma solução para resolver este problema seria fazer uma dll via windows form que retorne os dados do Web API.
