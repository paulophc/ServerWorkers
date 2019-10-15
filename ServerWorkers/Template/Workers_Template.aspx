<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Workers_Template.aspx.vb" Inherits="ServerWorkers.Workers_Template" %>

<!DOCTYPE html>

<html>
<head>
    <title>Workers Template</title>
    <link href="css/geral.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <!-- Painel dos Templates -->
    <div id="Templates" style="display: none">
        <!-- Template dos botoes YesNoCancel -->
        <div id="YesNoCancel_template" class="panel_message" style="display: none">
            <label id="YesNoCancelLabel" style="font-size: large"></label>
            <button id="YesButton" type="button" class="button_bold" style="font-size: large; height: 40px; min-width: 60px; margin-left: 10px">Sim</button>
            <button id="NoButton" type="button" class="button_bold" style="font-size: large; height: 40px; min-width: 60px; margin-left: 10px">Não</button>
            <button id="CancelButton" type="button" class="button_bold" style="font-size: large; height: 40px; min-width: 60px; margin-left: 10px">Cancelar</button>
        </div>

        <button id="button_template" type="button" class="button_bold" style="height: 40px; display: inline"></button>

        <!-- Template das Paginas -->
        <table id="Paginas_table_template" style="display: none;">
            <tr id="Pagina_tr_h">
                <th id="Pagina_Selecao_th">
                    <label id="Pagina_Selecao_h"></label>
                </th>
                <th id="Pagina_Info_th" class="cell_left_header" colspan="4">
                    <label id="Pagina_Info_h">Pagina</label>
                </th>
            </tr>
            <tr id="Pagina_tr1_h">
                <td id="Pagina_Null_th" rowspan="2">
                    <label id="Pagina_Null_h"></label>
                </td>
            </tr>
            <tr id="Pagina_tr">
                <td id="Pagina_Selecao_td" class="cell_center_itens">
                    <img id="Pagina_Selecao" src="images/navigate-right-icon.png" width="30" height="30" style="cursor: pointer" />
                </td>
                <td id="Pagina_Info_td" class="cell_left_itens" colspan="4" style="font-weight: bold; font-size: large">
                    <label id="Pagina_Info"></label>
                </td>
            </tr>
            <tr id="Pagina_tr1">
                <td id="Pagina_Null_td" rowspan="2">
                    <label id="Pagina_Null"></label>
                </td>
            </tr>
        </table>

        <!-- Template dos Workers -->
        <table id="Workers_table_template" style="display: none;">
            <tr id="Work_tr_h">
                <th id="Work_Selecao_th" class="cell_null">
                    <label id="Work_Selecao_h"></label>
                </th>
                <th id="Work_Nome_th" class="cell_left_header">
                    <label id="Work_Nome_h">Nome</label>
                </th>
                <th id="Work_Email_th" class="cell_left_header">
                    <label id="Work_Email_h">Email</label>
                </th>
                <th id="Work_Cargo_th" class="cell_left_header">
                    <label id="Work_Cargo_h">Cargo</label>
                </th>
                <th id="Work_Salario_th" class="cell_center_header">
                    <label id="Work_Salario_h">Salário</label>
                </th>
                <th id="Work_Data_th" class="cell_center_header">
                    <label id="Work_Data_h">Data da Contratação</label>
                </th>
            </tr>
            <tr id="Work_tr">
                <td id="Work_Selecao_td" class="cell_image">
                    <img id="Work_Selecao" src="images/Perspective-Button-Go-icon.png" class="image_inside_td" height="30" width="30">
                </td>
                <td id="Work_Nome_td" class="cell_center_itens">
                    <input id="Work_Nome" type="text" maxlength="100" size="50" placeholder="Nome" spellcheck="false" autocomplete="off" class="cell_input_left" />
                </td>
                <td id="Work_Email_td" class="cell_center_itens">
                    <input id="Work_Email" type="text" maxlength="100" size="30" placeholder="Email" spellcheck="false" autocomplete="off" class="cell_input_left" />
                </td>
                <td id="Work_Cargo_td" class="cell_center_itens">
                    <input id="Work_Cargo" type="text" maxlength="100" size="30" placeholder="Cargo" spellcheck="false" autocomplete="off" class="cell_input_left" />
                </td>
                <td id="Work_Salario_td" class="cell_right_itens">
                    <input id="Work_Salario" type="text" maxlength="10" size="10" placeholder="Salário" spellcheck="false" autocomplete="off" class="cell_input_right" />
                </td>
                <td id="Work_Data_td" class="cell_center_itens">
                    <input id="Work_Data" type="text" maxlength="10" size="10" placeholder="Data de Contratação" spellcheck="false" autocomplete="off" class="cell_input_center" />
                </td>
            </tr>
            <tr id="Work_Opcoes_tr">
                <td id="Work_Opcoes_td" class="cell_opcoes">
                    <button id="Work_Adicionar" type="button" class="button_image_left" style="float: none; background-image: url('../images/data-icon.png')"></button>
                    <button id="Work_Remover" type="button" class="button_image_left" style="float: none; background-image: url('../images/DeleteRed-icon.png')"></button>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
