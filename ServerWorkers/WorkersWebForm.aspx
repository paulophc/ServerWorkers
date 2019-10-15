<%@ Page Async="false" Language="vb" AutoEventWireup="false" CodeBehind="WorkersWebForm.aspx.vb" Inherits="ServerWorkers.WorkersWebForm" %>

<!DOCTYPE html>

<html>
<head>
    <title>Workers</title>
    <script src="js/jquery-1.9.1.js"></script>
    <script src="js/jquery.ui.core.js"></script>
    <script src="js/jquery.ui.widget.js"></script>
    <script src="js/jquery.ui.position.js"></script>
    <script src="js/jquery.ui.autocomplete.js"></script>
    <script src="js/jquery.ui.menu.js"></script>
    <script src="js/jquery.ui.mouse.js"></script>
    <script src="js/jquery.ui.touch-punch.min.js"></script>
    <script src="js/jquery.ui.draggable.js"> </script>
    <script src="js/jquery.ui.droppable.js"> </script>
    <script src="js/jquery.ui.resizable.js"></script>
    <script src="js/detectmobilebrowser.js"></script>
    <script src="js/jquery.jqpagination.js"></script>
    <script src="js/shared.js"></script>
    <meta name="viewport" content="width=device-width" />
    <link href="css/jquery-ui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="css/geral.css" rel="stylesheet" type="text/css" />
    <link href="css/slidemenu.css" rel="stylesheet" />
    <link href="css/windows8.css" rel="stylesheet" type="text/css" />
    <link href="css/jqpagination.css" rel="stylesheet" />
    <link rel="icon" href="data:;base64,iVBORw0KGgo=">
</head>
<body>

    <header>
        <!-- Painel HeaderPanel (Botoes do painel de titulos) -->
        <div id="HeaderPanel" style="display: none; padding: 5px">
            <img id="FieldInfoButton" title="Help" src="images/Interrogation-icon.png" width="45" height="45" style="cursor: pointer; display: inline-block; float: left" />
            <button id="SaveButton" title="Salvar" type="button" class="button_image_left" style="background-image: url('../images/Green-Sync-icon.png')"></button>
            <img id="FixMenuContainerButton" title="Desfixar ou Fixar o painel na parte inferior da janela" src="images/navigate-down-icon.png" width="45" height="45" style="cursor: pointer; display: none;" />
            <input id="Work_Filter_Descricao" type="text" maxlength="100" size="80" placeholder="Nome" spellcheck="false" autocomplete="off" class="text_left" style="padding-bottom: 4px; display: inline-block; float: left" />
            <div id="Pagination" title="Paginação" class="pagination" style="display: none">
                <a href="#" class="first" data-action="first">&laquo;</a>
                <a href="#" class="previous" data-action="previous">&lsaquo;</a>
                <input id="Block_Pages" type="text" readonly="readonly" />
                <a href="#" class="next" data-action="next">&rsaquo;</a>
                <a href="#" class="last" data-action="last">&raquo;</a>
            </div>
        </div>
    </header>

    <!-- Painel MainPanel (Container Principal) -->
    <section class="main">
        <div id="MainPanel" class="panel_main">
            <!-- Painel MessagesPanel (usado para exibir as mensagens na parte superior) -->
            <div id="MessagesPanel">
            </div>

            <!-- Painel ParametrosPanel (container dos elementos) -->
            <div id="ParametrosPanel" style="display: none; margin-top: 0px">
                <div id="Workers" style="margin-top: 4px; margin-bottom: 6px">
                    <table id="Workers_table">
                    </table>
                </div>
            </div>
        </div>
    </section>

    <!-- Painel para exibicao da imagem em full screen -->
    <div id="dialog" class="panel_window">
        <img id="Full_Image" src="" alt="" style="cursor: pointer;" />
    </div>

    <!-- Painel p/ imagem dinamica de preloader semelhante ao do windows 8 | class="windows8" -->
    <div id="preloader" style="display: none">
        <div class="wBall">
        </div>
        <div class="wBall">
        </div>
        <div class="wBall">
        </div>
        <div class="wBall">
        </div>
        <div class="wBall">
        </div>
    </div>
</body>
</html>
