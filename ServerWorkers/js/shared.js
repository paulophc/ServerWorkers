var box_selector = "input[type='text'], input[type='password'], input[type='checkbox'], textarea, select"
var fsize = ['xx-large', 'x-large', 'large', 'larger', 'medium', 'small', 'smaller', 'x-small', 'xx-small']
var fpoints = [24, 18, 14, 13.5, 12, 10, 10, 7.5, 7]
var fratio = [0.4, 0.5, 0.56, 0.5, 0.5, 0.7, 0.7, 0.8, 0.8] //Recalibrado p/ x-lager/large/medium
var htbl_buttons_default = { false: "images/Perspective-Button-Go-icon.png", true: "images/Perspective-Button-Stop-icon.png" }
var htbl_buttons_selected = { false: "images/Perspective-Button-Reboot-icon.png", true: "images/Perspective-Button-Shutdown-icon.png" }
var yesno_showing = false
var last_focus = ""
var block_pages_start_name = ""
var tema, cmips, cmdis, cmpi, cmtis, cmbis, cmris
var jfunctions = []

$(document).ready(function ()
{
    // Seta jfunctions c/ as funcoes que serao processadas no retorno do callback (Success)
    Set_jfunctions()

    // Chama Ajax CallBack para obter a lista inicial de Selects e Options e em seguida processar algumas funções
    // Verifica se ha informacoes em localStorage (gravadas de outras paginas). Se houver envia o caller_event como Start_QS ao inves de Start.
    Collect_Params("Start", (localStorage.getItem("QS_" + Get_PageName())) ? "Start_QS" : "Start", true, true)

    // Associa o evento click para os links relacionados ao menu
    // Teste -(atualmente esta selecionando todos os links). 
    $('a').not('.first, .previous, .next, .last').click(function (e) { e.preventDefault(); Collect_Params(this.id, "menu_button", true, false) });

    // Associa o evento click para o menu de slide
    $('.menu-anchor').on('click touchstart', function (e)
    {
        $('html').toggleClass('menu-active');
        e.preventDefault();
    });

    // Associa o evento click para os buttons relacionados ao login de usuarios. 
    $('#LogoffButton, #RemoveSessionButton, #CadUserButton, #AlterLoginButton, #UserId_Img').click(function (e) { e.preventDefault(); Collect_Params(this.id, "user_button", true, false) });

    // Exibe ou oculta os buttons relacionados ao Login
    $("#UserId_Img").click(function (e)
    {
        e.preventDefault();

        var u_buttons = "#LogoffButton, #RemoveSessionButton, #CadUserButton, #AlterLoginButton"
        var u_hide = $("#AlterLoginButton").get(0).style.display == "none"

        if (u_hide) { $(u_buttons).show() } else { $(u_buttons).hide() }
    });

    // Associa o evento click p/ os buttons Processar, Excluir e Limpar
    $('#SaveButton, #DeleteButton, #MoreInfoButton, #BackButton').click(function (e) { e.preventDefault(); Collect_Params(this.id, "button", true, (this.id == "SaveButton")) });

    // Associa o evento click p/ ESCButton, c/ preview = true quando a pagina e' Movimentos
    $('#EscButton').click(function (e) { e.preventDefault(); Collect_Params(this.id, "button", true, (Get_PageName() != "Movimentos.aspx")) });

    // Associa o evento click para FieldInfoButton p/ enviar o id e obter a informacao do campo. Usa tambem p/ a exportacao via Excel.
    $('#FieldInfoButton, #ExcelExportButton').click(function (e) { e.preventDefault(); Collect_Params(this.id, last_focus, true, (this.id == "ExcelExportButton")) });

    // Associa o evento p/click p/ FixMenuContainerButton
    // Define a altura do MainPanel, e associa o evento resize p/ manter sempre a mesma altura com o HeaderPanel fixado na parte superior na 'window'.
    $('#FixMenuContainerButton').click(function (e)
    {
        e.preventDefault();
        Fix_HeaderPanel(($(this).attr("src") == "images/navigate-up-icon.png"))
    });

    // Associa o evento click para ocultar o div c/ class panel_window
    $('#Full_Image').click(function (e) { e.preventDefault(); $('.panel_window').hide(function () { $(this).attr("src", ""); }); });

    // Associa o evento click para ImgFileButton para exibir VideoUrl e ThumbUrl
    $('#ImgFileButton, #ExcelImportButton').click(function (e) { e.preventDefault(); $("#" + ((this.id == "ImgFileButton") ? "ImgFile" : "xlFile")).click() });

    // Associa o evento change p/ ler arquivo c/ FileReader e em seguida chamar UploadFile
    $('#ImgFile, #xlFile').change(function (e)
    {
        e.preventDefault;

        var file, isImage
        var files = this.files

        for (var i = 0; i <= files.length - 1; i++)
        {
            file = files[i]
            isImage = file.type.indexOf("image") >= 0

            if (isImage || checkfile_extension(file.name, new Array(".xlsx", ".xls", ".csv")))
            {
                if (file.size <= 20971520)  // 20Mb
                {
                    UploadFile(file, isImage, (i == files.length - 1))
                }
                else
                {
                    Collect_Params('FileSizeAlert', file.name, false, false)
                }
            }
            else
            {
                Collect_Params('FileFormatAlert', file.name, false, false)
            }
        }
        $(this).val('') //Funciona como reset, caso contrario ao tentar enviar o mesmo arquivo novamente o evento nao e' acionado.
    });

    $(document).keyup(function (e) { if (e.keyCode == 27) { Collect_Params("EscButton", "Esc", true, false) } });

    // Associa beforeunload p/ efetuar uma chamada callback p/ Browser_Close p/ notificar o WebServer do fechamento da pagina.
    $(window).on("beforeunload", function (e) { e.preventDefault(); Perform_CallBack(Get_PageName() + "/Browser_Close", "{'token':'" + Get_Token() + "'}", false) })

    // Define a altura do painel, e associa o evento resize p/ manter a mesma altura.
    Fix_HeaderPanel(true)
});

// ------------------ Funções relacionadas 'a Focus, Blur, EnterTab -------------------

// Associa o evento Focus p/ as tags input, select, textarea e checkbox para alterar color e backgroud-color e remover o placeholder dos selects
function Set_Focus(newtags)
{
    var item, element

    for (var i = 0; i <= newtags.length - 1; i++)
    {
        item = newtags[i]
        element = $("#" + item.id)
        element.off("focus", Focus_function).on("focus", Focus_function)

        if (element.is(":checkbox")) { element.off('switchChange.bootstrapSwitch', Select_Change_function).on('switchChange.bootstrapSwitch', { params: item.params, send_token: item.token, preloader: item.preview }, Select_Change_function) }
    }
}

// Associa o evento Focus p/ as tags input, select, textarea e checkbox para alterar color e backgroud-color e remover o placeholder dos selects
function Focus_function()
{
    if ($(this).is("select"))
    {
        $("#" + this.id + " option[value='phd']").remove()

        // Como no IE mesmo apos definindo a propriedade css do elemento select os "options" ainda ficam
        // com as cores previas. Para solucionar este problema foi adicionado a instrucao abaixo, porem
        // ainda fica um efeito colateral que tambem so' ocorre no IE. (No desktop ao clicar no elemento select
        // o box de opcoes nao e' aberto automaticamente, porem, no ambiente touchscreen este efeito nao ocorre. 
        Set_Color($("#" + this.id + " option"), "default", true, true, false)
    }
    else if ($(this).is("input[type=text]"))
    {
        this.select()
    }

    Box_effects()

    Set_Color($(this), "default_focus", true, true, false)
}

// Associa o evento change p/ a tag select ou checkbox
function Select_Change_function(event)
{
    if (event.data.params)
    {
        Collect_Params(this.id, "change", event.data.send_token, event.data.preloader);
    }
    else
    {
        Collect_Param_Element($(this), "change", event.data.send_token, event.data.preloader);
    }

    // Verifica background-image de SaveButton, if "Grey" change to "Green"
    SaveButton_Image_Check()
}

// Associa o evento Focus p/ as tags relacionadas ao autocomplete para inicializar as informacoes do tipo e codigo.
// Esta instrucao foi adicionada para certificar-se de que o tipo e codigo estejam vazios, por exemplo caso o usuario apenas tecle <enter> no campo c/ autocomplete e este mesmo campo
// ja tenha uma informacao previa. Nestes casos o autocomplete nao e' "acionado" e consequentemente o tipo e codigo nao sao inicializados, enviando p/ servidor informacoes relacionadas a outro autocomplete nestes campos.
function Focus_codigo() { $("#tipo, #codigo").val("") }

// Associa o evento Blur p/ as tags input, select, textarea e checkbox para alterar o background-color para white
function Set_Blur(newtags)
{
    var item
    for (var i = 0; i <= newtags.length - 1; i++)
    {
        item = newtags[i]
        $("#" + item.id).off("blur", Blur_function).on("blur", Blur_function)
    };
}

// Associa o evento Blur p/ as tags input e select para alterar o background-color para white
function Blur_function()
{
    last_focus = ($(this).is("select, input, textarea")) ? $(this).attr("id") : ""
    $(box_selector).each(function ()
    {
        if (!($(this).get(0).style.color == "red"))
        {
            Set_Color($(this), "default", false, true, false)
        }
    });
}

// Associa os eventos keypress e keydown p/ as tags input e select (para detectar o pressionamento do Enter e Tab)
function Set_EnterTab(newtags)
{
    var item

    for (var i = 0; i <= newtags.length - 1; i++)
    {
        item = newtags[i]
        $("#" + item.id).off("keydown", EnterTab_function).on("keydown", { params: item.params, send_token: item.token, preloader: item.preview }, EnterTab_function)
    }

    //if (jQuery.browser.mozilla)
    //{
    //    $("#" + item.id).keypress(EnterTab_function)
    //}
    //else
    //{
    //    $("#" + item.id).keydown(EnterTab_function)
    //}
}

// Função usada para que o Enter se comporte como Tab
function EnterTab_function(e)
{
    if (e.keyCode == 13 || (e.keyCode == 9 && !e.shiftKey))
    {
        var element = $(this)
        if (!element.is("textarea") || (element.is("textarea") && element.val() == ""))
        {
            e.preventDefault();

            if (e.data.params)
            {
                Collect_Params(this.id, "enter", e.data.send_token, true, e.data.preloader)
            }
            else
            {
                Collect_Param_Element(element, "enter", e.data.send_token, e.data.preloader)
            }

            // Para solucionar fechamento do menu do autocomplete, quando e' digitada a informacao completa sem o uso do menu.
            if (element.hasClass("ui-autocomplete-input")) { element.autocomplete("close") }

            // Verifica background-image de SaveButton, if "Grey" change to "Green"
            SaveButton_Image_Check()

            // Posiciona no proximo elemento
            if (e.keyCode == 13 || e.keyCode == 9) { Goto_Next_Element(this) }
        }
        else
        {
            var lines = element.val().split(/\r|\r\n|\n/);
            var br_rows = 0
            var new_text = ""

            // Verifica no elemento textarea se ha 2 LFs teclados seguidamente no final
            // prepara o valor removendo os LFs extras e envia a informacao p/ o Server.
            for (var i = lines.length - 1; i >= 0; i--)
            {
                if (lines[i].trim() == "")
                {
                    br_rows += 1;
                    if (br_rows == 2)
                    {
                        // Se o textarea = space(1), posiciona no proximo elemento
                        if (element.val().replace("\r", "").replace("\n", "") != " ")
                        {
                            // Reatualiza o valor no elemento textarea p/ remover os LF extras. Caso contrario,
                            // ao posicionar novamente no campo e teclar <Enter> no final do conteudo, serao computados os LFs informados anteriormente.
                            for (x = 0; x <= i - 1; x++) { new_text += lines[x] + "\n" }
                            element.val(new_text)

                            Collect_Param_Element(element, "enter", e.data.send_token, e.data.preloader);
                            Goto_Next_Element(this) // Posiciona no proximo elemento
                            break;
                        }
                        else
                        {
                            Goto_Next_Element(this) // Posiciona no proximo elemento
                            break;
                        }
                    }
                }
                else
                {
                    break
                }
            }

        }
    }
};

// Posiciona no proximo elemento
function Goto_Next_Element(element)
{
    var nextElement = $('[tabindex="' + (element.tabIndex + 1) + '"]');
    if (nextElement.length) { nextElement.focus() } else { $('[tabindex="1"]').focus() };
}

// ------------- Algumas funções relacionadas ao 'Success' (retorno de informacoes do Server via Ajax CallBack) -------------

// Processa alguns comandos usando eval. Usado por exec e exec_firt. 
// exec_first e' usada sempre que for necessario processar algum comando antes de qualquer outro comando;
// por exemplo p/ remover todas as tr ou remover os options da tag select c/ empty() e etc.
// exec e' usado p/ processar informacoes apos execucao de outras funcoes no 'Success'
function Execs(exec)
{
    for (var i = 0; i <= exec.length - 1; i++) { eval(exec[i]) }
}

// Remove o elemento (usado para remover as linhas de Endereços, Fones, Emails e etc.)
function Set_Remove(remove)
{
    for (var i = 0; i <= remove.length - 1; i++)
    {
        var tr, table_name

        tr = $(remove[i]).first().is("tr")

        table_name = (tr) ? $(remove[i]).closest('table').attr("id") : null

        $(remove[i]).remove()

        if (tr) { round_table_corner(table_name) }
    }
}

// Remove as linhas que estao relacionadas à linha do drilldown.
function Remove_dd(itens_dds)
{
    var sline, cmdi
    for (var i = 0; i <= itens_dds.length - 1; i++)
    {
        sline = split_csv(itens_dds[i])  // tbl;parent1;parent2
        cmdi = get_cmdi(sline[0])

        $("#" + sline[1]).nextUntil("#" + sline[2]).filter('tr').not("tr[id*='_trp_']").remove()
        round_table_corner(cmdi.tbl_referencia + "_table")
    };
}

// Remove as tr relacionadas à linha do htbl
function Remove_linha(remove_lines)
{
    var sline, cmdi
    for (var i = 0; i <= remove_lines.length - 1; i++)
    {
        sline = split_csv(remove_lines[i])  // tbl;linha
        cmdi = get_cmdi(sline[0])

        if (sline[0] != "TNCMPIs" && sline[0] != "TNOPPIs")
        {
            $("tr[id^='" + cmdi.start_name + "_']").filter("tr[id$='" + sline[1] + "']").remove()
        }
        else // P/ nao remover _tr0_
        {
            $("tr[id^='tr1_" + cmdi.start_name + "_']").filter("tr[id$='tr1_" + sline[1] + "']").remove()
        }
        round_table_corner(cmdi.tbl_referencia + "_table")
    };
}

// Exibe ou oculta o elemento conforme a opção
function Set_Hide(hide)
{
    var sline
    for (var i = 0; i <= hide.length - 1; i++)
    {
        sline = split_csv(hide[i]) // id;True/False
        if (sline[1] == "True" || sline[1] == "true") { $("#" + sline[0]).hide() } else { $("#" + sline[0]).show() }
    };
}

// Define as informações dos selects (options)
function Set_Combo(combo)
{
    var selectIDs = [];
    var aOptions, html_string, sline

    // Obtem os ids das selects sem duplicados e armazena na array selectIDs
    for (var i = 0; i <= combo.length - 1; i++)
    {
        sline = split_csv(combo[i]) // id;value;text
        if (selectIDs.indexOf(sline[0]) < 0) { selectIDs.push(sline[0]) }
    };

    // Remove os options se houver 2 ou mais options.
    // Caso haja somente 1 option e este for placeholder, mantem os options,
    // ja que em alguns casos, e' serializado do servidor, somente os placeholders.
    for (var i = 0; i <= selectIDs.length - 1; i++)
    {
        aOptions = $.grep(combo, function (e) { return e.startsWith(selectIDs[i]) });

        if (aOptions.length > 1 || (aOptions.length == 1 && aOptions[0].indexOf(";phd;") < 0))
        {
            $("#" + selectIDs[i]).empty();
        }
    };

    // Adiciona o option na select correspondente.
    for (var i = 0; i <= combo.length - 1; i++)
    {
        sline = split_csv(combo[i]) // id;value;text

        if ($("#" + sline[0] + " option[value='" + sline[1] + "']").length == 0)
        {
            html_string = "<option value='" + sline[1] + "'>" + sline[2] + "</option>"

            if (sline[1] == "phd")
            {
                $("#" + sline[0]).prepend(html_string)
            }
            else
            {
                $("#" + sline[0]).append(html_string)
            }
        }
    };
}

// Define as informações dos inputs e selects
function Set_Values(value)
{
    var sline, element

    for (var i = 0; i <= value.length - 1; i++)
    {
        sline = split_csv(value[i]) // id;value
        element = $("#" + sline[0])
        element.val(sline[1])

        if ($(document.activeElement).is(element) && element.is("input[type=text]"))
        {
            element.select()
        }
    };
}

// Define as informações dos labels
function Set_Text(text)
{
    var sline
    for (var i = 0; i <= text.length - 1; i++)
    {
        sline = split_csv(text[i]) // id;text
        $("#" + sline[0]).text(sline[1])
    };
}

// Define os atributos usando .prop p/ placeholder, src, readonly, size, maxlength e etc.
function Set_Prop(prop)
{
    var sline
    for (var i = 0; i <= prop.length - 1; i++)
    {
        sline = split_csv(prop[i]) // id;atributo;value (obs: quando inicia c/ . usa css selector)
        $((sline[0].startsWith(".") ? "" : "#") + sline[0]).prop(sline[1], sline[2] != "false" ? sline[2] : false) // testa false p/ readonly e etc.
    };
}

// Altera a propriedade do css
function Set_Css(css)
{
    var sline

    for (var i = 0; i <= css.length - 1; i++)
    {
        sline = split_csv(css[i]) // selector;property;value
        $(sline[0]).css(sline[1], sline[2])
    };
}

// Adiciona ou Remove a tr de opcoes
function trOptions_Check(trOptions)
{
    var sline

    for (var i = 0; i <= trOptions.length - 1; i++)
    {
        sline = split_csv(trOptions[i]) // add;tbl;linha
        if (sline[0] == 'true') { Add_trOptions(sline[1], sline[2]) } else { Remove_trOptions(sline[1], sline[2]) }
    }
}

// Adiciona a tr de Opcoes
function Add_trOptions(tbl, linha)
{
    var cmdi, cmdo, id_selecao, opc_selector, trs_lines, tr_option
    var tbl_exceptions = ['TNCMPIs', 'TNCMAIs', 'TNCMVIs', 'TNOPFIs', 'TNOPPIs']

    cmdi = get_cmdi(tbl)
    cmdo = get_cmdi(tbl_exceptions.indexOf(tbl) >= 0 ? cmdi.tbl : cmdi.tbl_referencia)
    id_selecao = cmdi.start_name + "_" + (cmdi.itens.image ? "Img" : "Selecao") + "_" + linha
    opc_selector = "#" + cmdo.start_name + "_Opcoes_" + cmdi.start_name + "_tr_" + linha // Compoe o nome c/ cmdi.start_name, p/ diferenciar as 'tr' quando ha 'tr' de diferentes tbls na mesma table (Ex: Fornec/PFC ou Empresas/PPC e etc.)

    // Inicializa trs_lines com as linhas semelhantes
    trs_lines = $("#" + cmdi.table_name + " tr[id$='_" + linha + "']").filter("tr[id^='" + cmdi.start_name + "_']")

    // Define tr_option c/ o template dos Options e parametriza, conforme as informacoes dos parametros de trOptions
    tr_option = $(cmdo.trOptions.template)
    tr_option.attr("id", opc_selector.slice(1))
    tr_option.find("#" + cmdo.start_name + "_Opcoes_td").attr("colspan", total_colspan(trs_lines, false))
    tr_option.find(eval(cmdi.trOptions.tagsToHide)).css("display", "none")

    Set_Color(tr_option, "default", false, true, false)

    // E' necessario pegar a ultima linha semelhante, pois a linha de dados pode ser composta de 2 ou mais 'tr' ou
    // as linhas de dados podem ter as mesmas numeracoes de linhas, porem c/ dados diferentes (Ex: Fornec 0001/PFC 0001)
    tr_option.insertAfter(trs_lines.last())

    // Associa o evento click aos buttons ou imgs da tr adicionada.
    $(opc_selector + ' button, ' + opc_selector + ' img').click(function (e)
    {
        e.preventDefault()

        Collect_Params(this.id, id_selecao, true, false)

        if (this.id.indexOf("Image") < 0)
        {
            Remove_trOptions(tbl, linha)
        }
        else
        {
            // Para exibir imagens "fixa" o max-width do table, caso contrario se houver grande qtde. de imagens o layout e' alterado estendendo as celulas.
            $("#" + cmdi.table_name).css("max-width", $("#" + cmdi.table_name).width())
        }
    });

    var newtags = []
    $(opc_selector + " input[type='text'], " + opc_selector + " select").each(function ()
    {
        newtags.push({ "id": this.id, "params": true, "token": true, "preview": true })
    })

    // Associa o Focus, Blur e KeyDown p/ inputs e selects da tr adicionada.
    if (newtags.length)
    {
        // Seta determinada tag com focus
        $("#" + newtags[0].id).focus()

        // Efetua chamadas p/ Set_EnterTab, Set_Focus e Set_Blur p/ associar os eventos keypress, keydown, focus e blur c/ os elementos de newtags
        Set_NewTags(newtags)

        // Define algumas propriedades de Cores para os inputs, selects e textarea
        Box_effects()
    }

    //Se nao usar imagens, define a imagem c/ o botao default, caso o botao selected esteja definido.
    if (!cmdi.itens.image) { $("#" + id_selecao).attr("src", htbl_buttons_selected[cmdi.itens.red_button]) }

    round_table_corner(cmdi.tbl_referencia + "_table")
}

// Remove a tr de Opcoes
function Remove_trOptions(tbl, linha)
{
    var cmdi, cmdo, id_selecao, opc_selector
    var tbl_exceptions = ['TNCMPIs', 'TNCMAIs', 'TNCMVIs', 'TNOPFIs', 'TNOPPIs']

    cmdi = get_cmdi(tbl)
    cmdo = get_cmdi(tbl_exceptions.indexOf(tbl) >= 0 ? cmdi.tbl : cmdi.tbl_referencia)
    id_selecao = cmdi.start_name + "_" + (cmdi.itens.image ? "Img" : "Selecao") + "_" + linha
    opc_selector = "#" + cmdo.start_name + "_Opcoes_" + cmdi.start_name + "_tr_" + linha // Compoe o nome c/ cmdi.start_name, p/ diferenciar as 'tr' quando ha 'tr' de diferentes tbls na mesma table (Ex: Fornec/PFC ou Empresas/PPC e etc.)

    $(opc_selector).remove()

    //Se nao usar imagens, define a imagem c/ o botao default, caso o botao selected esteja definido.
    if (!cmdi.itens.image) { $("#" + id_selecao).attr("src", htbl_buttons_default[cmdi.itens.red_button]) }

    round_table_corner(cmdi.tbl_referencia + "_table")
}

// Define o atributo tabindex dos elementos
function Set_TabIndex(tabindex)
{
    var item
    var index = 1

    for (var i = 0; i <= tabindex.length - 1; i++)
    {
        item = tabindex[i]
        $("#" + item.id).attr('tabindex', (item.hide) ? -1 : index)
        index += (item.hide) ? 0 : 1
    };
}

// Seta determinada tag com o focus
function Set_Next_Focus(next_focus)
{
    if (next_focus == "MessagesPanel" || next_focus.match("_h3"))
    {
        var wtag = ($("#FixMenuContainerButton").attr("src").indexOf("up-icon")) >= 0 ? "html, body" : "#MainPanel"
        var msg_panel_offset = parseInt($("#MainPanel").scrollTop()) + parseInt($("#" + next_focus).offset().top) - $("#HeaderPanel").height() - 10 // 10 p/ adicionar um "padding"
        $(wtag).animate({ scrollTop: msg_panel_offset }, 'slow');
    }
    else
    {
        var element = $("#" + next_focus)
        element.focus()

        if (element.is("input[type=text]")) { element.select() }

        if (!element.is("button")) { Set_Color(element, "default_focus", true, true, false) }
    }
}

// Efetua chamadas p/ Set_EnterTab, Set_Focus e Set_Blur p/ associar os eventos keypress, keydown, focus e blur c/ os elementos de newtags
function Set_NewTags(newtags)
{
    // Associa os eventos keypress e keydown para a função EnterTab_function (para detectar o pressionamento do Enter e Tab)
    Set_EnterTab(newtags)

    // Associa o evento Focus p/ as tags input e select para alterar a classe, color e backgroud-color
    Set_Focus(newtags)

    // Associa o evento Blur p/ as tags input e select para alterar a o background-color para white
    Set_Blur(newtags)
}

// Associa a funcao autocomplete para alguns elementos
function Set_AutoCompl(autocompl)
{
    for (var i = 0; i <= autocompl.length - 1; i++)
    {
        if ($("#" + autocompl[i]).length > 0)
        {
            Set_Autocomplete(autocompl[i], Get_PageName() + "/Get_AutoComplete")
            $("#" + autocompl[i]).off("focus", Focus_codigo).on("focus", Focus_codigo)
        }
    };
}

// Habilita ou desabilita o autocomplete.
function Set_AutoCompl_Enable(autocompl_enable)
{
    var sline
    for (var i = 0; i <= autocompl_enable.length - 1; i++)
    {
        sline = split_csv(autocompl_enable[i]) // id;true/false
        $("#" + sline[0]).autocomplete(sline[1] == "true" ? "enable" : "disable")
    };
}

// Exibe o Painel de Mensagens com a mensagem recebida do retorno do CallBack
function Show_Msg(msg)
{
    var item, html_line, name_id_table

    for (var i = 0; i <= msg.length - 1; i++)
    {
        item = msg[i]
        name_id_table = ""

        $("#msgPanel_" + item.id).remove()

        if (!item.close)
        {
            if ($("#" + item.id).parent().is("td") && item.panelMsg == false)
            {
                // Inicializa name_id com  o nome da tr do id
                name_id = $("#" + item.id).parent().parent().attr("id")

                // Inicializa id_start_name com as iniciais do id (ex. table_tr1_0003 => table_tr1)
                // Foi adicionado slice(0,-1) p/ remover o ultimo caractere, (ex. table_tr1 => table_tr ou table_tr => table_t),
                // para que seja utilizada a 1a. linha como linha de referencia
                id_start_name = name_id.substring(0, name_id.lastIndexOf("_")).slice(0, -1)

                // Inicializa id_end_name com a final do id (ex. table_tr1_0003 => 0003)
                id_end_name = Get_Linha(name_id)

                // Inicializa name_id_table com o id do table
                name_id_table = $("#" + item.id).closest('table').attr("id")

                // Inicializa trs com as linhas semelhantes
                trs = $("#" + name_id_table + " tr[id$='" + id_end_name + "']").filter("tr[id^='" + id_start_name + "']")

                // Define html_line c/ o template da tr
                html_line = '<tr id ="msgPanel_' + item.id + '" display: none"><td colspan="' + total_colspan(trs, false) + '" style="padding: 3px;font-size:large"><label id="msgLabel_' + item.id + '">' + item.msg + '</label></td></tr>'

                $(html_line).insertBefore(trs.first())

                // Teste
                //if (isMobile())
                //{
                //    $(html_line).insertBefore(trs.first())
                //}
                //else
                //{
                //    $(html_line).insertAfter(trs.last())
                //}
            }
            else
            {
                // adiciona o elemento na <div> passada no parametro id
                html_line = '<div id="msgPanel_' + item.id + '" class="panel_message" style="display:none;font-size:large"><label id="msgLabel_' + item.id + '">' + item.msg + '</label></div>'
                var selector = "#" + ((item.panelMsg) ? "MessagesPanel" : $("#" + item.id).parent().attr("id"))

                $(html_line).prependTo(selector)

                // Teste
                //if (isMobile())
                //{
                //    $(html_line).prependTo(selector)
                //}
                //else
                //{
                //    $(html_line).appendTo(selector)
                //}
            }

            // obtem os elementos
            var msgPanel = $("#msgPanel_" + item.id) // Painel de mensagem
            var msgLabel = $("#msgLabel_" + item.id) // Label contendo a mensagem

            // obtem o tempo de exibição (acrescentando 2s para cada linha adicional)
            var msglines = (msgLabel.html() != undefined ? msgLabel.html().split("<br>").length : 1)
            var timeToDisplay = 4000 + (msglines * 3000)
            if (timeToDisplay > 60000) { timeToDisplay = 60000 } //Limita o tempo de exibicao p/ maximo de 1 minuto.

            // Define a cor do tema do painel de mensagens
            var msgColor = "msg_" + ((item.green) ? "green" : ((item.blue) ? "blue" : "red"))
            Set_Color($(msgPanel), msgColor, true, true, false)

            setTimeout(function ()
            {
                msgPanel.slideDown("slow", function ()
                {
                    if (name_id_table.length) { round_table_corner(name_id_table) }
                    if (item.tag_alert) { Set_Color($("#" + item.id), "alert", true, true, false) }
                })
            }, 0);
            setTimeout(function ()
            {
                msgPanel.slideUp("slow", function ()
                {
                    msgPanel.remove(); if (name_id_table.length) { round_table_corner(name_id_table) }
                })
            }, timeToDisplay);
        }
    };
};

// Exibe o Painel de Confirmacao c/ buttons
function Show_YesNoCancel(yesno)
{
    var yesno_template, item

    for (var i = 0; i <= yesno.length - 1; i++)
    {
        item = yesno[i]

        if (!yesno_showing)
        {
            yesno_showing = true

            yesno_template = $(get_cmti_template("YesNoCancel")).clone()
            yesno_template.attr('id', 'YesNoCancel')
            yesno_template.find("#YesNoCancelLabel").text(item.msg)
            yesno_template.find("#YesButton").attr("id", item.btn_yes).text(item.text_yes)
            if (item.show_no) { yesno_template.find("#NoButton").attr("id", item.btn_no).text(item.text_no) } else { yesno_template.find("#NoButton").remove() }
            if (item.show_cancel) { yesno_template.find("#CancelButton").attr("id", item.btn_cancel).text(item.text_cancel) } else { yesno_template.find("#CancelButton").remove() }

            if (isMobile())
            {
                yesno_template.prependTo("#" + item.id)
            }
            else
            {
                yesno_template.appendTo("#" + item.id)
            }

            // Associa o evento click para os buttons do yesno_template
            $('#YesNoCancel button').click(function (e)
            {
                e.preventDefault()

                Collect_Params(this.id, 'button', true, true)
                yesno_template.slideUp("slow", function () { yesno_template.remove(); yesno_showing = false })
            });

            setTimeout(function ()
            {
                yesno_template.slideDown("slow", function () { $("#" + item.focus).focus() })
            }, 0);
            setTimeout(function ()
            {
                yesno_template.slideUp("slow", function () { yesno_template.remove(); yesno_showing = false; Collect_Params(item.btn_cancel, "button", true, true) })
            }, item.duration);
        }
    };
};

// Verifica quais elementos (inputs, selects e textarea) ficarao c/ determinadas cores
// e chama Set_Color p/ efetivar as cores
function Box_effects()
{
    var plchold

    $(box_selector).each(function ()
    {
        if (!$(document.activeElement).is(this) && !($(this).get(0).style.color == "red")) // $(document.activeElement) 'elemento que esta com o focus
        {
            // plchold = true se o elemento for select verifica se o valor == placeholder, caso seja input verifica se o valor esta vazio.
            plchold = ($(this).is("select") ? $(this).val() == "phd" : !$(this).val())

            Set_Color($(this), plchold ? "placeholder" : "default", true, true, false)
        }
    });
}

// ------------------- Funções relacionadas aos buttons ------------------------

// Define os buttons e associa click event
function Layout_Buttons(cmd_btn)
{
    var gBtn, cmbi, ipath, item, btn_template, img_file, sline

    for (var i = 0; i <= cmd_btn.length - 1; i++)
    {
        gBtn = cmd_btn[i]  // gBtn = grupo de parametros relacionados aos buttons
        cmbi = get_cmbi(gBtn.grupo)
        ipath = get_ipath(cmbi.bpath)

        for (var j = 0; j <= gBtn.lBtns.length - 1; j++) //lBtns = lista de parametros de buttons relacionados ao grupo
        {
            item = gBtn.lBtns[j]
            btn_template = $(get_cmti_template(cmbi.template_id)).clone()
            btn_template.attr("id", item.id)
            if (item.text) { btn_template.text(item.text) }
            if (item.title) { btn_template.attr("title", item.title) }
            if (cmbi.classe) { btn_template.attr("class", cmbi.classe) }

            if (item.css_back_image)
            {
                img_file = (item.css_back_image.indexOf(":") >= 0 || item.css_back_image.indexOf("/") >= 0 ? item.css_back_image : ipath + item.css_back_image)
                btn_template.css({ 'background-color': 'transparent', 'background-position': 'center', 'background-size': 'cover', 'background-image': ((item.videourl) ? "url('" : "url('../") + img_file + "')" })
                btn_template.find('img').remove()
                btn_template.find('span').remove()
            }
            else
            {
                if (item.img || item.span)  // Usado no template de Cores
                {
                    if (item.img)
                    {
                        sline = split_csv(item.img) // 0 - id, 1 - src
                        img_file = (sline[1].indexOf(":") >= 0 || sline[1].indexOf("/") >= 0 ? sline[1] : ipath + sline[1])
                        btn_template.find('img').attr('src', img_file).attr('id', item.img.id)
                    }
                    else
                    {
                        btn_template.find('img').remove()
                    }

                    if (item.span)
                    {
                        sline = split_csv(item.span) // 0 - id, 1 - text
                        btn_template.find('span').text(sline[1]).attr('id', sline[0])
                    }
                    else
                    {
                        btn_template.css('background-color', 'transparent').find('span').remove()
                    }
                }
            }

            if (item.insertBefore) { btn_template.insertBefore('#' + item.insertBefore) }
            else if (item.appendTo) { btn_template.appendTo('#' + item.appendTo) }

            eval(cmbi.click_event) // Associa o evento click p/ efetuar a chamada Ajax CallBack p/ o Server

            if (item.css_back_image)
            {
                if (item.videourl) // Habilita o evento click para o video
                {
                    $("#" + item.id).click(function (e)
                    {
                        e.preventDefault();
                        Collect_Params(this.id, "cor_img", false, false)
                        window.open(item.videourl, "_blank")
                    });
                }
                else // Habilita o evento click para a Imagem
                {
                    $("#" + item.id).click(function (e)
                    {
                        e.preventDefault();
                        Collect_Params(this.id, "cor_img", false, false)
                        img_file = $(this).css("background-image").replace("Thumbs", "Original")
                        Show_Foto(img_file.slice(5, -2))
                    });
                }
            }
        }
    }
}

// -------------- Funções relacionadas aos Spans/Imgs (Sortable) -------------------

// Define os Spans e Imgs p/ Sortable
function Layout_Spans(cmd_span)
{
    var template_base, span_template, img_file, sline, linha, img, span, gSpan, ipath

    for (var i = 0; i <= cmd_span.length - 1; i++)
    {
        gSpan = cmd_span[i]  // gSpan = grupo de parametros relacionados aos spans

        ipath = get_ipath(gSpan.bpath)

        for (var j = 0; j <= gSpan.lSpans.length - 1; j++) //lSpans = lista de parametros de spans relacionados ao grupo
        {
            sline = split_csv(gSpan.lSpans[j]) // linha;img;span (text)
            linha = sline[0]
            img = sline[1]
            span = sline[2]

            template_base = $(get_cmti_template(gSpan.template_id).replace("></span>", ">" + span + "</span>"))
            span_template = template_base.clone()
            span_template.attr('id', gSpan.start_name + "_Order_" + linha)

            if (img.length)
            {
                img_file = (img.indexOf(":") >= 0 || img.indexOf("/") >= 0 ? img : ipath + img)
                span_template.find('img').attr('src', img_file).attr('id', gSpan.start_name + "_Order_Img_" + linha)
            }
            else
            {
                span_template.find('img').remove()
            }

            span_template.appendTo('#' + gSpan.appendTo)
        };

        // Ativa resizable
        $("#" + gSpan.appendTo).sortable({
            update: function (event, ui)
            {
                Collect_Params_Order(ui.item.attr("id"), "order", gSpan.appendTo)
                pauta_span(gSpan)

                // Verifica background-image de SaveButton, if "Grey" change to "Green"
                SaveButton_Image_Check()
            }
        });

        pauta_span(gSpan)
    }
}

// Define a cor da pauta do span
function pauta_span(gSpan)
{
    Set_Color($('#' + gSpan.appendTo + ' span:nth-child(odd)'), "default", true, true, false)
    Set_Color($('#' + gSpan.appendTo + ' span:nth-child(even)'), gSpan.span_color, true, true, false)
}

// ------------------- Funções relacionadas 'as imagens ------------------------

// Define as imagens e associa click event
function Layout_Imagens(cmd_img)
{
    var cmdi, gImg, bpath, item, img_size, img_file, html_line

    for (var i = 0; i <= cmd_img.length - 1; i++)
    {
        gImg = cmd_img[i]  // gImg = grupo de parametros relacionados aos imgs
        cmdi = get_cmdi(gImg.tbl)
        bpath = (gImg.tbl == "Produtos" ? "prod" : gImg.tbl == "Cadastros" ? "cad" : cmdi.bpath)

        for (var j = 0; j <= gImg.lImgs.length - 1; j++) //lImgs = lista de parametros de imgs relacionados ao grupo
        {
            item = gImg.lImgs[j]
            img_size = isMobile() ? gImg.size_mobile : gImg.size_desktop
            img_file = (item.src.indexOf(":") >= 0 || item.src.indexOf("/") >= 0 ? item.src : get_ipath(bpath) + item.src)
            html_line = '<img id="' + item.id + '" src="' + img_file + '"  width="' + img_size + '" height="' + img_size + '" style="margin:' + (gImg.margin ? gImg.margin : "0px 0px 0px 6px") + '; cursor: pointer" />'

            $(html_line).appendTo("#" + gImg.appendTo)

            if (gImg.draggable)
            {
                if ($("#ImagesButton").text() != "Imagens")  // Caso o usuario esteja no modo adicionar/remover imagens associa draggable ao elemento.
                {
                    $("#" + item.id).draggable({ appendTo: "body", helper: "clone" });
                }
            }

            // Habilita o evento click para as Imagens
            $("#" + item.id).click(function (e)
            {
                e.preventDefault();

                // Exibe o video ou a imagem
                if (item.video) { window.open(item.videourl, "_blank") } else { Show_Foto($(this).attr("src").replace("Thumbs", "Original")) }

                // Atualiza o src das imagens do table c/ o src da imagem selecionada pelo usuario.
                if (gImg.update_img)
                {
                    var element = $(this)
                    var tr_opcoes = element.closest("tr")

                    if (tr_opcoes.is("tr"))
                    {
                        var linha = Get_Linha($(tr_opcoes).attr("id"))
                        var img_id = cmdi.start_name + "_Img_" + linha

                        $("#" + img_id).attr("src", $(element).attr("src"))

                        Collect_Params(img_id, $(element).attr("id"), false, false)
                    }
                }
                else
                {
                    Collect_Params(this.id, "img_fullview", false, false)
                }
            });
        }
    }
}

// Exibe a foto em "FullScreen"
function Show_Foto(src)
{
    SetFullScreen(document.documentElement)
    $('.panel_window').css({ 'width': "100%", 'height': "100%" })
    $('#Full_Image').css({ 'width': "100%", 'height': "100%" }).attr("src", src)
    $("#dialog").css({ 'top': $(window).scrollTop(), 'left': $(window).scrollLeft() }).show("slow").focus()
};

// Associa o evento droppable p/ os elementos a serem usados no Drop (Drag & Drop) de Imagens
function Set_Droppable_Img(drop_selector, accept, caller_event)
{
    // Associa o evento droppable para ImgRemoveDragPanel
    $(drop_selector).droppable(
        {
            activeClass: "drag_panel_active",
            hoverClass: "drag_panel_hover",
            accept: accept,
            tolerance: "pointer",
            drop: function (event, ui)
            {
                $("#" + ui.draggable.attr("id")).attr("src", "")
                Collect_Params(ui.draggable.attr("id"), caller_event, true, false)
            }
        });
}

// ------------------- Funções relacionadas aos tables -------------------------

// Efetua chamadas p/ Layout ou Layout2 (posteriormente Layout2 sera removido)
function Pre_Layout(cmd)
{
    var item, cmdi

    for (var i = 0; i <= cmd.length - 1; i++)
    {
        item = cmd[i]
        cmdi = get_cmdi(item.tbl)

        if (item.tbl != "PrecoBriefs")
        {
            if (item.pages.length) { Layout_Pagina(cmdi, item) }
            Layout_Tables(cmdi, item)
        }
        else
        {
            Layout_Tables_2(cmdi, item)
        }
    }
}

// Layout de Pagina
function Layout_Pagina(cmdi, cmd)
{
    var trs_header, trs_itens, trs_lines, sline, pagina, descricao, qtd_rows, focus

    $("#" + cmdi.table_name + " tr").remove()

    trs_header = $(cmpi.template_header).filter("#Pagina_tr_h").clone()
    trs_header.attr("id", cmdi.start_name + "_trp_header")

    Set_Color(trs_header, "cell_header", true, true, false)

    trs_header.appendTo("#" + cmdi.table_name)

    for (var i = 0; i <= cmd.pages.length - 1; i++)
    {
        sline = split_csv(cmd.pages[i])
        pagina = parseInt(sline[0]) // Integer
        descricao = sline[1] // String
        qtd_rows = parseInt(sline[2]) // Integer
        focus = (sline[3] == "True")  // Boolean

        trs_itens = $(cmpi.template_itens).filter("#Pagina_tr").clone()
        trs_itens.attr("id", cmdi.start_name + "_trp_" + pagina)
        trs_itens.find("#Pagina_Selecao").attr("id", cmdi.start_name + "_Pagina_Selecao_" + pagina)
        trs_itens.find("#Pagina_Info").text(cmpi.descPage + " " + ((pagina < 1000) ? zfill(pagina, 3) : pagina) + " (" + ((qtd_rows < 1000) ? zfill(qtd_rows, 3) : qtd_rows) + " " + (qtd_rows == 1 ? cmpi.descLine : cmpi.descLines) + ")" + ((descricao) ? " - " + descricao : "")).attr("id", cmdi.start_name + "_Pagina_Info_" + pagina)

        Set_Color(trs_itens, "cell", true, true, false)

        trs_itens.appendTo("#" + cmdi.table_name)

        // Associa o evento click p/ Efetuar a chamada Ajax CallBack p/ o Server
        $("#" + cmdi.start_name + "_Pagina_Selecao_" + pagina).click(function (e) { e.preventDefault(); Collect_Params(this.id, "sort", false, true) });

        // Altera a imagem da selecao de navigation-right p/ navigation-down
        if (focus) { $("#" + cmdi.start_name + "_Pagina_Selecao_" + pagina).attr("src", "images/navigate-down-icon.png") }
    }

    // Pauta as linhas da pagina
    trs_lines = $("#" + cmdi.table_name + " tr").slice(1).filter("tr:nth-child(odd)")

    Set_Color(trs_lines, "pauta_page", false, true, false)

    round_table_corner(cmdi.tbl_referencia + "_table")
}

// Layout das Tabelas
function Layout_Tables(cmdi, cmd)
{
    var trs_header, trs_lines, trs_lines_lenght, trs_filter, last_tr_header, last_line_id, trs_lines_default
    var id, linha, sline, sColumns, index, value, has_header, ph, sel, seli, sline_sel, sline_opt, index_hsel, ipath, cmip, insAfter

    if ($("#" + cmdi.table_name).css('display') == 'none') { $("#" + cmdi.table_name).show() }

    // Remove as 'tr' se cmd.header.clear = true
    if (cmd.pages.length == 0 && cmd.clear) { $("#" + cmdi.table_name + " tr").remove() }

    has_header = $("#" + cmdi.table_name + " th[id^='" + cmdi.start_name + "_']").length > 0

    if (cmdi.header.show && !has_header)
    {
        // Inicializa trs_header com as 'tr' do header do template e em seguida ajusta algumas informacoes (attr, hide, adiciona td de pagina se for o caso e etc.)
        trs_header = $(cmdi.header.template).clone()
        trs_header = Layout_Ajust_Linhas(cmdi, cmd, trs_header, true)

        // Altera o id das 'tr' e dos 'th' e define a propriedade css.
        for (var i = 0; i <= trs_header.length - 1; i++)
        {
            tr = trs_header[i]
            $(tr).attr("id", $(tr).attr("id") + "eader")

            $(tr).find("th").each(function (index, th)
            {
                $(th).attr("id", $(th).attr("id") + "eader")
                $(th).children().attr("id", $(th).children().attr("id") + "eader")
            });
        }

        Set_Color(trs_header, "cell_header", true, true, false)

        // Obtem a ultima tr do header
        last_tr_header = $("#" + cmdi.table_name + " tr[id$='header']").last()

        // Adiciona as linhas do header no data_table
        if (last_tr_header.length > 0) { trs_header.insertAfter(last_tr_header) } else { trs_header.appendTo("#" + cmdi.table_name) }

        // Processa os eventos associados aos labels, imgs e etc.
        if (cmdi.header.click_event) { eval(cmdi.header.click_event) }
    }

    // Inicializa last_line_id com null
    last_line_id = null

    // Inicializa trs_lines com as 'tr' das linhas do template e em seguida ajusta algumas informacoes (attr, hide, adiciona td de pagina se for o caso e etc.)
    trs_base = $(cmdi.itens.template)
    trs_base = Layout_Ajust_Linhas(cmdi, cmd, trs_base, false)
    sColumns = split_csv(cmdi.columns) // colunas da tabela

    //Inicializa bpath c/ o path dos botoes c/ imagens
    ipath = get_ipath(cmdi.bpath)

    for (var i = 0; i <= cmd.htbl.length - 1; i++)
    {
        sline = split_csv(cmd.htbl[i])
        linha = sline[0] // a primeira column e' usada para indicar a linha

        trs_lines = trs_base.clone()

        trs_lines.each(function (index, tr)
        {
            $(tr).attr("id", $(tr).attr("id") + "_" + linha)

            $(tr).find("td").each(function (index, td)
            {
                id = $(td).attr("id").slice(0, -3)
                tag = $(td).find("#" + id)

                if (tag.length > 0)
                {
                    index = sColumns.indexOf(id) + 1 //Obtem o indice da coluna relacionada e acrescenta + 1, pois a 1a. ocorrencia e' da linha
                    if (index > 0)
                    {
                        value = sline[index]

                        if (tag.is("input[type='text']") || tag.is("textarea") || tag.is("select"))
                        {
                            if (tag.is("select"))
                            {
                                seli = $.grep(cmdi.sels, function (e) { return e.id == id })[0]

                                if (seli.options.length > 0)
                                {
                                    var asel = $.grep(cmd.hsel, function (e) { return e.startsWith(id + "_" + linha) })[0]

                                    if (asel != undefined)
                                    {
                                        sline_sel = split_csv(asel)

                                        if (sline_sel.length > 1)
                                        {
                                            sline_opt = split_csv(sline_sel[1])
                                            sline_opt.unshift("phd")

                                            for (var j = 0; j <= sline_opt.length - 1; j++)
                                            {
                                                sel = $.grep(seli.options, function (e) { return e.value == sline_opt[j] })[0]
                                                if (sel != undefined) { $(tag).append($('<option>', { value: sel.value, text: sel.text })); }
                                            }
                                        }
                                        else
                                        {
                                            sel = $.grep(seli.options, function (e) { return e.value == value })[0]
                                            if (sel != undefined) { $(tag).append($('<option>', { value: sel.value, text: sel.text })); }
                                        }
                                    }
                                    else
                                    {
                                        for (var j = 0; j <= seli.options.length - 1; j++)
                                        {
                                            $(tag).append($('<option>', { value: seli.options[j].value, text: seli.options[j].text }));
                                        }
                                    }
                                    $(tag).val(value)
                                }
                            }
                            else
                            {
                                iph = $.grep(cmdi.iph, function (e) { return e.id == id })[0]
                                if (iph.length) { $(tag).attr('placeholder', iph.iph) }

                                $(tag).val(value)
                            }
                        }
                        else if (tag.is("label") || tag.is("button") || tag.is("span"))
                        {
                            $(tag).text(value)
                        }
                        else if ($(tag).is(':checkbox'))
                        {
                            $(tag).prop('checked', (value == "True" || value == "true")).bootstrapSwitch()
                        }
                        else if (tag.is("img"))
                        {
                            if (value.length)
                            {
                                $(tag).attr("src", value.indexOf(":") >= 0 || value.indexOf("/") >= 0 ? value : ipath + value)
                            }
                            else
                            {
                                $(tag).attr("src", htbl_buttons_default[cmdi.itens.red_button])
                            }
                        }
                    }
                    $(tag).attr("id", id + "_" + linha)
                }
            });
        });

        Set_Color(trs_lines, "cell", true, true, false)

        // Verifica cmd.insertAfter p/ inicializar last_line_id c/ id p/ ser usado como insertAfter
        // InsertAfter e' usado somente quando ha' 2 ou mais tabelas relacionadas (Ex: Fornec/PFCs, Empresa/PPCs, PFiltros/PFIs e etc.),
        // Conforme o exemplo acima as tables PFCs, PPCs e PFIs terao insertAfter p/ posicionar a primeira linha do item logo apos a linha correspondente da tabela master 
        insAfter = $.grep(cmd.insertAfter, function (e) { return e.startsWith(linha) })[0]
        if (insAfter != undefined) { last_line_id = "#" + split_csv(insAfter)[1] }

        // Adiciona as linhas ao table conforme criterios das instrucoes abaixo
        if (cmd.insertAfter_page) //InsertAfter_page e' usado p/ posicionar o elemento na pagina correspondente
        {
            trs_lines.insertAfter("#" + cmd.insertAfter_page)
            cmd.insertAfter_page = null
        }
        else if (last_line_id)
        {
            trs_lines.insertAfter(last_line_id)
        }
        else
        {
            trs_lines.appendTo("#" + cmdi.table_name)
        }

        // Inicializa last_line_id c/ id da ultima linha inserida no table
        last_line_id = "#" + trs_lines.last().attr("id")

        // Processa os eventos associados aos buttons, imgs e etc.
        if (cmdi.itens.click_event) { eval(cmdi.itens.click_event) }
    }

    // Processa function p/ ajuste do layout da tabela
    if (cmd.layout_rows_check) { eval(cmd.layout_rows_check) }

    // Pauta o table.
    pauta_table(cmdi.table_name)

    // Se houver paginas, ajusta o colspan da pagina.
    if (cmdi.tbl == cmdi.tbl_referencia && $("#" + cmdi.table_name + " th[id^='Pagina_Info']").length > 0)
    {
        // Obtem as linhas relacionadas, p/ calcular o total_colspan.
        // -1 e' usado p/ subtrair o colspan correspondente ao drilldown.
        var trs = $("#" + cmdi.table_name + " tr[id$='_header']").not("tr[id*='_trp_']")
        if (trs.length) { $("#" + cmdi.table_name + " th[id^='Pagina_Info'], td[id^='Pagina_Info']").attr('colspan', total_colspan(trs, true) - (cmdi.drilldown ? 1 : 0)) }
    }

    // Ativa resizable
    if (cmdi.resize) { $('th, td').resizable({ handles: 'e' }); }

    // Arredonda os corners 
    round_table_corner(cmdi.tbl_referencia + "_table")
}

// Layout das Tabelas 2 (Works like Pivot table) - remover posteriormente
function Layout_Tables_2(cmdi, cmd)
{
    var trs_header, trs_lines, trs_filter, count
    var id, linha, sline, sColumns, index, value, has_header

    sColumns = split_csv(cmdi.columns) // colunas da tabela

    // Remove as 'tr' se cmd.header.clear = true
    if (cmd.pages.length == 0 && cmd.clear) { $("#" + cmdi.table_name + " tr").remove() }

    if (cmdi.header.show)
    {
        // Inicializa trs_header com as 'tr' do header do template e em seguida ajusta algumas informacoes (attr, hide, adiciona td de pagina se for o caso e etc.)
        trs_header = $(cmdi.header.template)
        trs_header = Layout_Ajust_Linhas(cmdi, cmd, trs_header, true)

        trs_header.each(function (index, tr)
        {
            $(tr).attr("id", $(tr).attr("id") + "eader")
            th_td = $(tr).find("th").clone()
            $(tr).find("th").remove()
            id = $(th_td).attr("id").slice(0, -3)

            $.each(cmd.htbl, function (index, line)
            {
                sline = split_csv(line)
                linha = sline[0] // a primeira column e' usada para indicar a linha

                th = th_td.clone().attr("id", id + "_th_" + linha)
                tag = $(th).children()

                if (tag.length > 0)
                {
                    index = sColumns.indexOf(id) + 1 //Obtem o indice da coluna relacionada e acrescenta + 1, pois a 1a. ocorrencia e' da linha
                    if (index > 0)
                    {
                        value = sline[index]

                        if (tag.is("input[type='text']") || tag.is("textarea") || tag.is("select"))
                        {
                            $(tag).val(value)
                        }
                        else if (tag.is("label") || tag.is("button") || tag.is("span"))
                        {
                            $(tag).text(value)
                        }
                        else if ($(tag).is(':checkbox'))
                        {
                            $(tag).prop('checked', (value == "True" || value == "true")).bootstrapSwitch()
                        }
                        else if (tag.is("img"))
                        {
                            $(tag).attr("src", (value.length) ? value : htbl_buttons_default[cmdi.itens.red_button])
                        }
                    }
                    $(tag).attr("id", id + "_" + linha)
                    th.appendTo($(tr))
                }
            });
        });

        Set_Color(trs_header, "cell_header", true, true, false)

        trs_header.appendTo("#" + cmdi.table_name)
    }

    // Inicializa trs_lines com as 'tr' das linhas do template e em seguida ajusta algumas informacoes (attr, hide, adiciona td de pagina se for o caso e etc.)
    trs_lines = $(cmdi.itens.template)
    trs_lines = Layout_Ajust_Linhas(cmdi, cmd, trs_lines, false)

    trs_lines.each(function (index, tr)
    {
        th_td = $(tr).find("td").clone()
        $(tr).find("td").remove()
        id = $(th_td).attr("id").slice(0, -3)

        $.each(cmd.htbl, function (index, line)
        {
            sline = split_csv(line)
            linha = sline[0] // a primeira column e' usada para indicar a linha

            td = th_td.clone().attr("id", id + "_td_" + linha)
            tag = $(td).children()

            if (tag.length > 0)
            {
                index = sColumns.indexOf(id) + 1 //Obtem o indice da coluna relacionada e acrescenta + 1, pois a 1a. ocorrencia e' da linha
                if (index > 0)
                {
                    value = sline[index]

                    if (tag.is("input[type='text']") || tag.is("textarea") || tag.is("select"))
                    {
                        $(tag).val(value)
                    }
                    else if (tag.is("label") || tag.is("button") || tag.is("span"))
                    {
                        $(tag).text(value)
                    }
                    else if (tag.is("img"))
                    {
                        $(tag).attr("src", (value.length) ? value : htbl_buttons_default[cmdi.itens.red_button])
                    }
                }
                $(tag).attr("id", id + "_" + linha)
                td.appendTo($(tr))
            }
        });
    });

    trs_lines.appendTo("#" + cmdi.table_name)

    Set_Color(trs_lines, "cell", true, true, false)

    // Pauta o table.
    pauta_table(cmdi.table_name)

    // Ativa resisable
    $('th, td').resizable({ handles: 'e' });

    // Arredonda os corners 
    round_table_corner(cmdi.tbl_referencia + "_table")
}

// Ajusta algumas informacoes do layout (attr, hide, adiciona td de pagina se for o caso e etc.)
function Layout_Ajust_Linhas(cmdi, cmd, trs, header)
{
    // Se houver dados em layout_check, efetua diversos ajustes em trs. Ex: MPDs, MPTs, MFPs e etc...
    if (cmd.layout_check) { eval("trs = " + cmd.layout_check) }

    // Se houver pagina, insere o td c/ o Label Null que fica alinhado c/ a Selecao da Pagina.
    if (cmd.pages.length)
    {
        clone_th = (header) ? $(cmpi.template_header).filter("#Pagina_Null_th").clone() : $(cmpi.template_itens).filter("#Pagina_Null_td").clone()
        clone_th.attr("rowspan", trs.length)
        clone_th.insertBefore(trs.first().find(((header) ? "th" : "td") + ":first"))
    }

    // Inicializa o atributo data-pauta c/ pauta_index (exceto header)
    if (!header) { trs.attr("data-pauta", cmdi.itens.pauta_index) }

    return trs
}

// Ajusta o width de algumas colunas do table, obtendo a row c/ maior quantidade de caracteres ou usando o height como referencia p/ width e etc.
// O resize nao e' processado no momento das tabelas, devido a informacoes que podem alterar o conteudo das celulas como por exemplo: Combos, Values, Text e etc.
// Como MPDs, MPDIs e MSTs usam celulas dinamicas, nao sao processados neste ponto.
function Htbl_Resize_Columns_Check(cmd)
{
    var item
    var tbl_exceptions = ["MPDs", "MPDIs", "MFPIs", "MSTs"]

    for (var i = 0; i <= cmd.length - 1; i++)
    {
        item = cmd[i]
        if (tbl_exceptions.indexOf(item.tbl) < 0)
        {
            Resize_Columns(get_cmdi(item.tbl), 5, 0)
        }
    }
}

// --------- Funcoes e Eventos associadas aos buttons dos tables ------------

// Habilita o evento click para exibir as imagens ou video em fullscreen.
function Set_Click_Image_Video(id, videourl)
{
    $("#" + id).click(function (e)
    {
        e.preventDefault();

        // Exibe o video ou a imagem
        if (videourl) { window.open(videourl, "_blank") } else { Show_Foto($(this).attr("src").replace("Thumbs", "Original")) }
    });
}

// Associa o evento click ao botao de selecao do CEP.
function Set_Click_CEP_Selecao(id)
{
    // Associa o evento click para p/ inicializar as informações do endereço e etc.
    $('#' + id).click(function (e)
    {
        e.preventDefault();

        // Em caller_event envia o id da opcao p/ obter a linha do endereco
        var opc_selector = "#" + $(this).parent().parent().parent().parent().parent().parent().parent().attr("id")
        var linha = Get_Linha(opc_selector)

        Collect_Params(this.id, linha, false, false)

        Remove_trOptions("Enderecos", linha, "")
    });
}

// Chama a function Set_Click_Drill_Down
function Click_Drill_Down(id, parent_1, parent_2, selector)
{
    $("#" + id).off('click').on('click', Set_Click_Drill_Down(id, parent_1, parent_2, selector))
}

// Associa o evento click para as setas usadas como selecao p/ drill-down
function Set_Click_Drill_Down(id, parent_1, parent_2, selector)
{
    $('#' + id).click(function (e)
    {
        e.preventDefault();

        var linha, direction

        linha = Get_Linha(this.id)
        direction = ($(this).attr("src").toLowerCase().indexOf("right") >= 0) ? "down" : "right"
        dd_status = (direction == "right") ? "close_drilldown" : "open_drilldown"

        Collect_Params(this.id, dd_status, false, (dd_status == "open_drilldown"))

        $(this).attr("src", "images/navigate-" + direction + "-icon.png")

        if (parent_1 != null && parent_2 != null)
        {
            if (dd_status == "open_drilldown") { $("#" + parent_1).nextUntil("#" + parent_2).filter("tr[id*='_Opcoes_']").remove(); }
            if (dd_status == "close_drilldown") { $("#" + parent_1).nextUntil("#" + parent_2).filter("tr").not("tr[id*='_trp_']").remove(); }
            round_table_corner($(this).closest('table').attr('id'))
        }
        else
        {
            // selector e' usado p/ casos especificos como o table de NCMs
            // que tem tables inside td, sendo necessario portanto remover as tr c/ estes tables.
            if (dd_status == "open_drilldown") { $(selector).remove(); $("tr[id*='_Opcoes_']").remove() }
            if (dd_status == "close_drilldown") { $(selector).remove() }
            round_table_corner($(this).closest('table').attr('id'))
        }
    });
}

// Associa o evento click para "ativar" o painel de opcoes das linhas dos tables
// Foi usado o evento click ao invez do evento toggle pois os buttons Adicionar/Remover e etc tambem colapsam o mesmo painel
function Set_Click_Opcoes_Linhas(tbl, linha, tagsToHide)
{
    var cmdi, cmdo, id_selecao, opc_selector, trs_options, alread_open, linha2
    var tbl_exceptions = ['TNCMPIs', 'TNCMAIs', 'TNCMVIs', 'TNOPFIs', 'TNOPPIs']

    cmdi = get_cmdi(tbl)
    cmdo = get_cmdi(tbl_exceptions.indexOf(tbl) >= 0 ? cmdi.tbl : cmdi.tbl_referencia)
    id_selecao = cmdi.start_name + "_" + (cmdi.itens.image ? "Img" : "Selecao") + "_" + linha
    opc_selector = "#" + cmdo.start_name + "_Opcoes_" + cmdi.start_name + "_tr_" + linha // Compoe o nome c/ cmdi.start_name, p/ diferenciar as 'tr' quando ha 'tr' de diferentes tbls na mesma table (Ex: Fornec/PFC ou Empresas/PPC e etc.)

    $("#" + id_selecao).click(function (e)
    {
        e.preventDefault();

        alread_open = false

        // Inicializa trs_options com as trs relacionadas ao option que estao "ativadas".
        trs_options = $("#" + cmdi.table_name + " tr[id^='" + cmdo.start_name + "_Opcoes_" + cmdi.start_name + "']")

        // Caso ja tenha "options ativados", fecha os options que estao "ativados".
        for (var i = 0; i <= trs_options.length - 1; i++)
        {
            linha2 = Get_Linha($(trs_options[i]).attr("id"))
            Remove_trOptions(tbl, linha2)

            if (linha == linha2) { alread_open = true }
        }

        if (!alread_open)  // Se nao houver "options ativados".
        {
            // Coleta os parametros e envia informacoes p/ o servidor p/ obter o template e os parametros p/ adicionar a tr de Opcoes.
            Collect_Params(id_selecao, "Add_trOptions", false, cmdi.itens.preview)
        }
    });
}

// Fecha todos os drilldown menu c/ excessao p/ o elemento onde focus_element e' diferente de MoreInfoButton
function Close_All_DrillDown(tbl, moreinfo, start_name_item, start_name_header, tag_tr1, tag_tr2, next_line2, rowspan)
{
    if (moreinfo)
    {
        $("#" + tag_tr1).nextUntil("#end_of_table").filter("tr[id^='" + start_name_item + "_']").not("tr[id$='header']").not("tr[id*='_trp_']").remove()
    }
    else
    {
        $("#" + tag_tr1).nextUntil("#" + tag_tr2).filter("tr[id^='" + start_name_item + "_']").not("tr[id$='header']").not("tr[id*='_trp_']").remove()

        if (next_line2 != null)
        {
            var tag_tr2 = start_name_header + "_tr" + rowspan + "_" + next_line2
            $("#" + tag_tr2).nextUntil("#end_of_table").filter("tr[id^='" + start_name_item + "_']").not("tr[id*='_trp_']").remove()
        }
    }
    $("#" + tbl + "_table tr[id*='" + start_name_header + "_Opcoes_']").remove()

    var cmdi = get_cmdi(tbl)
    round_table_corner(cmdi.tbl_referencia + "_table")
    Set_Next_Focus("MessagesPanel") // Para alinhar o header c/ top of window
}

// -------------- Funções relacionadas a alguns Templates -------------------

// Insere o template da linha vertical (vt - vertical line)
function insert_vt_template(insertAfter)
{
    $(get_cmti_template("vertical_line")).clone().insertAfter('#' + insertAfter)
}

// Insere o template de xlEstatisca (Informacoes de Importacao/Exportacao p/ Excel)
function insert_xlEstatistica_template()
{
    var cmbi = get_cmbi("xlEstatistica")
    $(get_cmti_template("xlEstatistica")).clone().insertBefore('#MessagesPanel')
    eval(cmbi.click_event) // Associa o evento click p/ efetuar a chamada Ajax CallBack p/ o Server
}

// Insere o template de DataHora em MSTs
function insert_datahora_template(data_hora, appendTo)
{
    var datahora_template = get_cmti_template("DataHora")
    datahora_template = datahora_template.replace("><", ">" + data_hora + "<")

    $(datahora_template).clone().appendTo('#' + appendTo)
}

// ------------------------ Funções genericas -------------------------------

// Função para verificar se é sistema operacional para dispositivo "movel"
function isMobile()
{
    // Teste quando puder testar no smartphone remover esta function e chamar diretamente a funcao abaixo
    return $.browser.mobile;
}

// Teste - Remover posteriormente quando puder testar no smartphone.
function rem_isMobile()
{
    var agents = ['android', 'webos', 'iphone', 'ipad', 'blackberry', 'windows phone'];
    for (var i = 0; i <= agents.length - 1; i++)
    {
        if (navigator.userAgent.toLowerCase().search(agents[i]) > 0)
        {
            return true;
        }
    }
    return false;
}

// Função para verificar se a string é composta por numeros
function isNumber(n)
{
    return !isNaN(parseFloat(n)) && isFinite(n);
}

// preenche o numero com zeros à esquerda
function zfill(num, len)
{
    return (Array(len).join("0") + num).slice(-len);
}

// Verifica se uma das extensoes passadas como array e' valida para o file_name
function checkfile_extension(file_name, validExts)
{
    var extension = file_name.toLowerCase().substring(file_name.lastIndexOf('.'));
    return (validExts.indexOf(extension) >= 0)
}

// Define a altura do MainPanel, e associa o evento resize p/ manter sempre a mesma altura com o HeaderPanel fixado na parte superior na 'window'.
function Fix_HeaderPanel(fix)
{
    if (fix)
    {
        if ($("#MainPanel").length && $("#HeaderPanel").length) // Testa se MainPanel e HeaderPanel existem (pois nem todas as paginas tem os dois paineis, Login e etc.)
        {
            MainPanel_Resize()
            $(window).on("resize", MainPanel_Resize);
        }
        $("#FixMenuContainerButton").attr("src", "images/navigate-down-icon.png")
    }
    else
    {
        $(window).off("resize", MainPanel_Resize)
        $("#MainPanel").height('auto')
        $("#FixMenuContainerButton").attr("src", "images/navigate-up-icon.png")
    }
}

// Redefine MainPanel.height on resize
function MainPanel_Resize()
{
    // Adicionado -10 p/ calibrar a altura do HeaderPanel.
    $("#MainPanel").height($(window).height() - $("#HeaderPanel").height() - 10)

    if (Get_PageName() == "Movimentos.aspx") { ItensPanel_ReDim_Height() }
}

// Associa o evento toggle para expandir ou colapsar
function Set_Toggle(tagclick, taganimate, vel)
{
    $('#' + tagclick).click(function (e)
    {
        e.preventDefault()

        isClicked = $(this).data('clicked');
        if (isClicked) { isClicked = false } else { isClicked = true }
        $(this).data('clicked', isClicked)

        if (isClicked)
        { $("#" + taganimate).slideDown(vel) }
        else
        { $("#" + taganimate).slideUp(vel) }
    });
}

// Associa o evento toggle_Panels p/ Collapsar ou Expandir paineis.
function toggle_Panels(button, panel1, panel2, command1, command2, vel)
{
    if ($("#" + panel1).is(":visible"))
    {
        $("#" + panel1).hide()
        $("#" + panel2).slideDown(vel, function ()
        {
            $("#" + panel2).show()
            eval(command1)
            Set_Next_Focus('MessagesPanel');
        })
    }
    else
    {
        $("#" + panel2).hide()
        $("#" + panel1).slideUp(vel, function ()
        {
            $("#" + panel1).show()
            eval(command2)
            Set_Next_Focus('MessagesPanel');
        })
    }
}

// Associa o evento jqPagination na div c/ a class .pagination
function Set_jqPagination(on, blp_start_name, current_page, max_page, max_rows)
{
    var pagination = $('.pagination')

    if (!block_pages_start_name)
    {
        block_pages_start_name = (blp_start_name) ? blp_start_name : "init"

        // Inicializa informacoes no controle jqPagination
        pagination.css('display', 'none').jqPagination(
            {
                max_page: max_page,
                current_page: current_page,
                page_string: 'Bloco {current_page}/{max_page}',
                paged: function (page) { Collect_Params(block_pages_start_name + "_Block_Pages_" + page, "sort", true, true) }
            });
    }
    else
    {
        if (on)
        {
            var value = (current_page == 1 && max_page == 1 ? '' : 'Bloco ' + current_page + '/' + max_page + ' de ') + max_rows + (max_rows == 1 ? ' linha' : ' linhas')

            block_pages_start_name = blp_start_name
            pagination.css('display', 'inline-block').jqPagination('option', { max_page: max_page, current_page: current_page, trigger: false })
            pagination.find('input').css('display', 'inline-block').width(textWidth('medium', value.length, 5, 30)).val(value)
        }
        else
        {
            pagination.css('display', 'none')
        }
    }
}

// Define propriedade de background-color, color para os elementos.
function Set_Color(element, option, setcolor, setbackcolor, pauta)
{
    // descricoes usadas em tema.colors.descricao
    // cell, cell_header, cell_selected, cell_header_selected, default, placeholder, alert
    // msg_red, msg_green, msg_blue, destak_blue, pauta_page, pauta1, pauta2, pauta3, pauta4 e etc...

    if (tema.colors != undefined)
    {
        if (!pauta) //Nao e' pauta
        {
            var cor = $.grep(tema.colors, function (e) { return e.descricao == option });

            if (cor.length)
            {
                if (setcolor) { element.css("color", cor[0].color) }
                if (setbackcolor)
                {
                    if (element.parent().is("td"))
                    {
                        // Verifica se a linha esta c/ os atributos de pauta, se sim usa backgroud-color do data-attribute ao inves de backcolor.
                        var td = element.parent()
                        var condition = (td.attr("data-backcolor") == undefined || option == "phd" || option == "default_focus")

                        element.css("background-color", (condition) ? cor[0].backcolor : td.attr("data-backcolor"))
                        td.css("background-color", (condition) ? cor[0].backcolor : td.attr("data-backcolor"))
                    }
                    else
                    {
                        element.css("background-color", cor[0].backcolor)
                    }
                }
            }
        }
        else //pauta
        {
            var pauta_index = element.first().attr("data-pauta")
            var cor = $.grep(tema.colors, function (e) { return e.descricao == ((option == "pauta") ? "pauta" + pauta_index : option) });

            if (cor.length)
            {
                // not("div") foi adicionado p/ nao pegar as divs relacionados ao "jq resizable" e 
                // evitar collateral effects (bordas laterais das celulas ficam "invisiveis"),
                var tags = element.find("td").children().not("div")

                tags.css("background-color", cor[0].backcolor)
                tags.parent().css("background-color", cor[0].backcolor).attr("data-backcolor", cor[0].backcolor)
                element.attr("data-backcolor", cor[0].backcolor)
            }
        }
    }
}

// Verifica background-image de SaveButton, if "Grey" change to "Green"
function SaveButton_Image_Check()
{
    if (/Grey/.test($("#SaveButton").css("background-image"))) { $("#SaveButton").css("background-image", "url('../images/Green-Sync-icon.png')") }
}

// Retorna a linha ou codigo que no final do valor passado como parametro
function Get_Linha(value)
{
    return value.substring(value.lastIndexOf("_") + 1)
}

// Retorna o array da linha que esta no formato CSV
function split_csv(line)
{
    var sline = []

    if (line)
    {
        var previous_char = ""
        var qmark = false
        var double_qmark = false
        var previous_double_qmark = false
        var delimiter = ";"
        var column = ""
        var c

        // qmark e' usado como flag de controle para saber se a column esta c/ quotation mark.
        // Ex: ABC;"DEF";GHI   ou   ABC;"Uma frase com ;";DEF
        // double_qmark e' usado como flag de controle para saber quando e' duplo quotation mark. Obs: Faz parte do protocolo CSV
        // Ex: ABC;"DEF "fone e email" GHI";JKL   ou    ABC;"Uma frase c/ ; e "";DEF

        for (var i = 0; i <= line.length - 1; i++)
        {
            c = line[i]

            if (c == '"')
            {
                if (double_qmark)
                {
                    column += c
                    double_qmark = false
                    previous_double_qmark = true
                }
                else
                {
                    if ((i == 0 || previous_char == delimiter) && !qmark)
                    {
                        qmark = true
                        double_qmark = false
                    }
                    else if (((i + 1) <= (line.length - 1)) && line[i + 1] == delimiter)
                    {
                        qmark = false
                        double_qmark = false
                    }
                    else
                    {
                        double_qmark = !(previous_char == '"' && !previous_double_qmark)
                    }
                    previous_double_qmark = false
                }
            }
            else if (c == delimiter && !qmark)
            {
                sline.push(column)
                column = ""
            }
            else
            {
                column += c
            }
            previous_char = c
        }
        sline.push(column)
    }
    return sline
}

// Esta function efetua um replace de double, single quotes, e retorna o valor formatado no padrao csv.
// foi criada a funcao pois a instrucao: value = value.replace('"', '""')
// nao adiciona dois duble quotes quando ha' double quote no final da string, e nem mesmo usando \".
// " p/ "" e' usado no padrao csv e ' p/ || e' para manter a string compativel c/ o json.
// e se houver delimitador (";") no texto, retorna o value entre double quotes.
function convert_to_csv(value)
{
    var new_value = ""
    var delimiter = false

    if (value != null)
    {
        for (var i = 0; i <= value.length - 1; i++)
        {
            if (value[i] == '"')
            {
                new_value += '""'
            }
            else if (value[i] == "'")
            {
                new_value += '||'
            }
            else
            {
                new_value += value[i]
            }
            if (value[i] == ";") { delimiter = true }
        }
    }

    return (delimiter ? '"' + new_value + '"' : new_value)
}

// Retorna o Token
function Get_Token()
{
    return (localStorage.getItem("token")) ? convert_to_csv(localStorage.getItem("token")) : "null"
}

// Retorna o nome da pagina
function Get_PageName()
{
    return location.pathname.substring(location.pathname.lastIndexOf("/") + 1)
}

// Muda p/ fullscreen mode
// Find the right method, call on correct element
function SetFullScreen(element)
{
    if (element.requestFullscreen)
    {
        element.requestFullscreen();
    } else if (element.mozRequestFullScreen)
    {
        element.mozRequestFullScreen();
    } else if (element.webkitRequestFullscreen)
    {
        element.webkitRequestFullscreen();
    } else if (element.msRequestFullscreen)
    {
        element.msRequestFullscreen();
    }
}

// Whack fullscreen
function ExitFullScreen()
{
    if (document.ExitFullScreen)
    {
        document.ExitFullScreen();
    } else if (document.mozCancelFullScreen)
    {
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen)
    {
        document.webkitExitFullscreen();
    }
}

//Retorna o nivel de Zoom (Chrome)
function Get_Zoom_Level()
{
    var r = (window.outerWidth - 16) / window.innerWidth
    var snaps = [0.29, 0.42, 0.58, 0.71, 0.83, 0.95, 1.05, 1.18, 1.38, 1.63, 1.88, 2.25, 2.75, 3.5, 4.5, 100]
    var ratios = [0.25, '1/3', 0.5, '2/3', 0.75, 0.9, 1, 1.1, 1.25, 1.5, 1.75, 2, 2.5, 3, 4, 5]
    var i, result
    for (var i = 0; i < 16; i++) { if (r < snaps[i]) return eval(ratios[i]); }
}

// Ajusta o width de algumas colunas, obtendo a row c/ maior quantidade de caracteres ou usando o height como referencia p/ width e etc.
function Resize_Columns(cmdi, min_length, extra_length)
{
    // th_td       - usado no selector sera' igual a "th" ou "td" (td e' usado nos casos de tables sem headers)
    // sColumns    - Nome das colunas (sem o no. da linha ou theader)
    // MinLengths  - Qtde. de caracteres minimos p/ determinada coluna. 
    //               E' usado quando ha outras tbls, pois a(s) coluna(s) que estao no mesmo alinhamento tem um minimo de caracteres
    //               maior que o minimo de caracteres da tbl 'master'. Obs: linhas contem: id;value
    // MinLengths  - Qtde. de caracteres maximos p/ determinada coluna. 
    //               E' usado para limitar o tamanho da coluna como por exemplo na tabela de IBPTs que tem colunas somente c/ labels
    //               c/ "VarChar(1000)" ou na tabela MST onde DataHora deve ser limitado somente p/ conter a largura da Data. Obs: linhas contem: id;value
    // min_length  - E' usado p/ informar a quantidade minima de caracteres padrao.
    //               quando aMinLengths nao especificar a quantidade minima de caracteres.
    //extra_length - E' usado quando ha' colunas dinamicas (podem ser exibidas ou ocultas), como por exemplo, Tamanhos de MPDs;
    //               Ex. e' informado a quantidade de caracteres minimos que as demais colunas precisam ter (No. de colunas * quantidade minima de caracteres)

    if (cmdi.fitcontent)
    {
        var elements, selector, wlength, twidth, w, max_length, th_td, smin, smax, sColumns, ncols, vmin_length, id_first_tr, elength

        ncols = 0
        twidth = 0
        vmin_length = min_length
        th_td = (cmdi.header.show) ? "th" : "td"
        sColumns = split_csv(cmdi.columns)

        if (cmdi.drilldown) { sColumns.unshift(cmdi.start_name + "_DrillDown") }
        if (sColumns.indexOf(cmdi.start_name + "_Img") < 0) { sColumns.unshift(cmdi.start_name + "_Selecao") }

        for (var i = 0; i <= sColumns.length - 1; i++)
        {
            column_name = sColumns[i]
            elements = $("#" + cmdi.table_name + " td:not(:hidden)[id^='" + column_name + "']").find('input, select, label, img, textarea, :checkbox')

            if (elements.length > 0)
            {
                id_first_tr = elements[0].closest("tr").id

                if (id_first_tr.indexOf("_tr_") >= 0 || id_first_tr.indexOf("_tr1_") >= 0) // P/ computar somente os tds da primeira tr.
                {
                    smin = $.grep(cmdi.itens.MinLengths, function (e) { return e.startsWith(column_name) });
                    min_length = (smin.length) ? parseInt(split_csv(smin[0])[1]) : vmin_length

                    smax = $.grep(cmdi.itens.MaxLengths, function (e) { return e.startsWith(column_name) });
                    max_length = (smax.length) ? parseInt(split_csv(smax[0])[1]) : 250

                    selector = "#" + cmdi.table_name + " " + th_td + "[id^='" + column_name + "']"
                    wlength = (th_td == "th") ? $(selector).children().text().length : min_length

                    if (elements.first().is("input, textarea"))
                    {
                        for (var j = 0; j <= elements.length - 1; j++)
                        {
                            elength = $(elements[j]).val().length
                            wlength = elength > wlength ? elength : wlength
                        }
                        w = textWidth("large", wlength, min_length, max_length)
                    }
                    else if (elements.first().is("select"))
                    {
                        for (var j = 0; j <= elements.length - 1; j++)
                        {
                            elength = $(elements[j]).find(":selected").text().length
                            wlength = elength > wlength ? elength : wlength
                        }
                        w = textWidth("large", wlength, min_length, max_length)
                    }
                    else if (elements.first().is("label"))
                    {
                        for (var j = 0; j <= elements.length - 1; j++)
                        {
                            elength = $(elements[j]).text().length
                            wlength = elength > wlength ? elength : wlength
                        }
                        w = textWidth("medium", wlength, min_length, max_length)
                    }
                    else if (elements.first().is("img"))
                    {
                        w = elements.first().height()  // Como altura e largura sao iguais usa height como referencia, visto que, width esta' c/ 100%
                    }
                    else if (elements.first().is(":checkbox"))
                    {
                        w = textWidth("large", 5, 0, max_length)
                    }

                    $(selector).css('width', w + "px")
                    twidth += w
                    ncols += 1
                }
            }
        }

        if (twidth > 0)
        {
            // (ncols * 2) e' usado p/ computar os 'pixels' das bordas das celulas.
            var table_width = twidth + (ncols * 2) + (extra_length > 0 ? textWidth("large", extra_length, 0, 2000) : 0)
            $("#" + cmdi.table_name).css('width', table_width + "px")
        }
    }
    else if (cmdi.resize && (cmdi.table_width))
    {
        $("#" + cmdi.table_name).css('width', cmdi.table_width)
    }
}

// Calcula a largura do texto em points
function textWidth(font_size, wlength, min_length, max_length)
{
    // Retorna min_length * Get_Zoom_Level se a wlength for menor que min_length ou
    //         max_length * Get_Zoom_Level se a wlength for maior que max_length
    // Usa fratio p/ calibrar a largura, conforme o tipo de fonte.
    // var fpixels = [32, 24, 19, 18, 16, 13, 13, 10, 9]
    var l, i, fpt, w

    l = (wlength > min_length ? wlength : min_length)
    l = (l < max_length ? l : max_length)
    i = fsize.indexOf(font_size)
    fpt = (fpoints[i] * l) * fratio[i]
    w = Math.round(fpt * Get_Zoom_Level() * (isMobile() ? 1.4 : 1))

    return w;
}

// Obtem o cmdi de cmdis usando $.grep (cmdi = parametros relacionado 'as tabelas)
function get_cmdi(tbl)
{
    return $.grep(cmdis, function (e) { return e.tbl == tbl })[0];
}

// Obtem o template de cmtis usando $.grep (cmti = parametros relacionado aos templates)
function get_cmti_template(id)
{
    var cmti = $.grep(cmtis, function (e) { return e.id == id })[0];
    return (cmti.template)
}

// Obtem o cmbi de cmbis usando $.grep (cmbi = parametros relacionado aos buttons (template_id, click_event, classe))
function get_cmbi(grupo)
{
    return $.grep(cmbis, function (e) { return e.grupo == grupo })[0];
}

//Retorna o path das imagens, conforme o tipo indicado em bpath
function get_ipath(bpath)
{
    var ipath = ""

    if (bpath != undefined)
    {
        var cmip = $.grep(cmips, function (e) { return e.bpath == bpath })[0]
        if (cmip != undefined) { ipath = cmip.ipath }
    }

    return ipath
}

//Retorna o array contendo campo e campos relacionados
// Esta array e' utilizada, geralmente quando ha' autocomplete.
// Nestes casos ao inves de enviar todos os dados, envia-se somente alguns campos que estao relacionado ao caller_id
// Por exemplo c/ Nome ou Nome Reduzido de Cadastros deve ser enviado tambem o tipo e o codigo.
function get_crs(campo)
{
    if (isNumber(campo.slice(-4)))
    {
        var linha = Get_Linha(campo)
        campo = campo.replace("_" + linha, "")
    }

    return $.grep(cmris, function (e) { return e.campo == campo })
}

// Funcao para arrendondar ou alinhar os cantos dos tables
// Observar se algum div parent correspondente nao esta hidden, neste caso
// nao sera processado corretamente, pois e' feito filtro p/ td (not :hidden)
function round_table_corner(table_name)
{
    // Inicializa linhas com a quantidade de linhas (trs) do table
    var linhas = $("#" + table_name + " tr").length

    // Processa somente se houver linhas
    if (linhas > 0)
    {
        // Remove os round corner previamente, (adicionado p/ prever adicao de nova(s) linha(s)), em seguida arredonda somente se 'tema.round = true'
        var all_td_first = $("#" + table_name + " tr").find("td:not(:hidden):first")
        var all_td_last = $("#" + table_name + " tr").find("td:not(:hidden):last")

        all_td_first.css({ 'border-radius': '0px 0px 0px 0px', '-moz-border-radius': '0px 0px 0px 0px' })
        all_td_last.css({ 'border-radius': '0px 0px 0px 0px', '-moz-border-radius': '0px 0px 0px 0px' })

        all_td_first.find("img").css({ 'border-radius': '0px 0px 0px 0px', '-moz-border-radius': '0px 0px 0px 0px' })
        all_td_last.find("img").css({ 'border-radius': '0px 0px 0px 0px', '-moz-border-radius': '0px 0px 0px 0px' })

        if (tema.round) // Arredonda os corners
        {
            var colLeftTop, colRightTop, colLeftBottom, colRightBottom, th_td
            var bLeftTop, bRightTop, bLeftBottom, bRightBottom
            var flag_lin, flag_Top, flag_Bottom, flag_Left, flag_Right

            // Se houver th (headers) usa th, se nao, usa td. E usado como parametro para obter o td ou th
            th_td = $("#" + table_name + " th").length > 0 ? "th" : "td"

            // Como o layout varia de tabela para tabela e em alguns layouts o last left td ou o last right td
            // nao e' o ultimo td, ja que o penultimo ou antepenultimo e etc, podem estar com o rowspan > 1
            // Portanto e' necessario verificar previamente as trs com estas informacoes de rowspan p/ obter a informacao correta

            // Inicializa name_id com  o nome da ultima tr
            name_id = $("#" + table_name + " tr:last").attr("id")

            // Inicializa id_start_name com as iniciais do id (ex. table_tr1_0003 => table)
            id_start_name = name_id.substring(0, name_id.indexOf("_"))

            // Inicializa id_end_name com as iniciais do id (ex. table_tr1_0003 => _0003)
            id_end_name = name_id.substring(name_id.lastIndexOf("_"))

            // Inicializa last_trs com as ultimas linhas semelhantes 
            last_trs = $("#" + table_name + " tr[id$='" + id_end_name + "']").filter("tr[id^='" + id_start_name + "']").not(":hidden")

            // Obtem os tds correspondentes aos corners da ultima linha
            colLeftBottom = last_trs.last().find("td:not(:hidden)").first()
            colRightBottom = last_trs.last().find("td:not(:hidden)").last()

            // Obtem os tds correspondentes que tem a informacao de rowspan
            crs_left = last_trs.find("td:not(:hidden):first[rowspan]")
            crs_right = last_trs.find("td:not(:hidden):last[rowspan]")

            // Verifica se a ultima linha nao e' uma linha c/ unico 'td' como por exemplo a tr de opcoes
            if ((colLeftBottom.attr("id") != colRightBottom.attr("id")) || (last_trs.length == crs_left.attr("rowspan") || last_trs.length == crs_right.attr("rowspan")))
            {
                // Caso nao obtenha rows c/ rowspan p/ direita ou esquerda, inicializa novamente.
                if (crs_left.length > 0) { colLeftBottom = crs_left }
                if (crs_right.length > 0) { colRightBottom = crs_right }
            }

            // Inicializa c/ os tds correspondentes aos corners do table
            colLeftTop = $("#" + table_name + " tr:first-child").find(th_td + ":not(:hidden)").first()
            colRightTop = $("#" + table_name + " tr:first-child").find(th_td + ":not(:hidden)").last()

            // Inicializa flag_lin c/ true se houver somente uma linha.
            // Inicializa flag_Top e flag_Bottom c/ true se houver um unico td p/ a linha ou se a ultima linha for a linha de opcoes.
            flag_lin = (linhas == 1)
            flag_Top = colLeftTop.attr("id") == colRightTop.attr("id")
            flag_Bottom = colLeftBottom.attr("id") == colRightBottom.attr("id")
            flag_Left = (linhas <= last_trs.length && colLeftTop.attr("id") == colLeftBottom.attr("id"))
            flag_Right = (linhas <= last_trs.length && colRightTop.attr("id") == colRightBottom.attr("id"))

            bLeftTop = '6px ' + (flag_Top ? '6px' : '0px') + ' 0px ' + (flag_lin ? '6px' : '0px')
            bRightTop = (flag_Top ? '6px' : '0px') + ' 6px ' + (flag_lin ? '6px' : '0px') + ' 0px'
            bRightBottom = '0px ' + ((flag_lin || flag_Right) ? '6px' : '0px') + ' 6px ' + (flag_Bottom ? '6px' : '0px')
            bLeftBottom = ((flag_lin || flag_Left) ? '6px' : '0px') + ' 0px ' + (flag_Bottom ? '6px' : '0px') + ' 6px'

            colLeftTop.css({ 'border-radius': bLeftTop, '-moz-border-radius': bLeftTop })
            colRightTop.css({ 'border-radius': bRightTop, '-moz-border-radius': bRightTop })
            colRightBottom.css({ 'border-radius': bRightBottom, '-moz-border-radius': bRightBottom })
            colLeftBottom.css({ 'border-radius': bLeftBottom, '-moz-border-radius': bLeftBottom })

            // Como alguns tables contem imagens arredonda tambem as imagens que estiverem nos 'corners'
            colLeftTop.find("img").css({ 'border-radius': bLeftTop, '-moz-border-radius': bLeftTop })
            colRightTop.find("img").css({ 'border-radius': bRightTop, '-moz-border-radius': bRightTop })
            colRightBottom.find("img").css({ 'border-radius': bRightBottom, '-moz-border-radius': bRightBottom })
            colLeftBottom.find("img").css({ 'border-radius': bLeftBottom, '-moz-border-radius': bLeftBottom })
        }
    }
}

// Retorna o numero total de colspan (columns) de um table, verificando as trs passadas como parametro
function total_colspan(trs, header)
{
    var colMax = 1
    var t_ = (header) ? "th" : "td"

    // trs contem as 'tr' com a mesma linha. E' necessario porque uma ou mais 'td' pode estar com
    // rowspan e ocupar a posicao de 2 ou mais 'td' da outra linha.
    trs.each(function (index, item)
    {
        var colCount = 0;
        $(item).find(t_ + ':not(:hidden)').each(function ()
        {
            if ($(this).attr('colspan'))
            {
                colCount += +$(this).attr('colspan');
            }
            else
            {
                colCount++;
            }

            if (colCount > colMax) { colMax = colCount }
        });
    });
    return colMax
}

// Pauta as linhas de itens do table
function pauta_table(table_name)
{
    var name_id, id_end_name, trs_lines, trs_lines_length, pauta, pauta_count

    for (var i = 1; i <= 7; i++) // 7 tipos diferentes de pautas
    {
        trs_lines = $("#" + table_name + " tr[data-pauta='" + i + "']")

        if (trs_lines.length > 0)
        {
            // Inicializa name_id com  o nome da ultima tr (sem as linhas do header e das paginas)
            name_id = trs_lines.last().attr("id")

            // Inicializa id_end_name com as iniciais do id (ex. table_tr1_0003 => _0003)
            id_end_name = name_id.substring(name_id.lastIndexOf("_"))

            // Obtem o numero de linhas, usando filter para saber as ultimas linhas semelhantes 
            trs_lines_length = $(trs_lines).filter("tr[id$='" + id_end_name + "']").length

            pauta_count = 1
            pauta = false
            pauta_started = false

            // Usa each ao inves de nth.child, pois nth.child selector, fica "referenciado" ao table,
            // e como o table tem linhas dinamicas (Opcoes, Adicao de linhas, Mensagens e etc.), ao "entrar" 1 dessas linhas,
            // o selector considera-as mesmo que trs_lines so' tenha as linhas a serem pautadas.

            trs_lines.each(function (index, tr)
            {
                if (index == trs_lines_length)
                {
                    pauta = true; pauta_started = true
                }
                else if (pauta_started)
                {
                    pauta_count += 1
                    if (pauta_count == trs_lines_length + 1) { pauta = !pauta; pauta_count = 1 }
                }

                Set_Color($(tr), (pauta) ? "pauta" : "default", false, true, true)
            });
        }
    }
}

// ------------------ Funções relacionadas ao Ajax CallBack -------------------

// Seta jfunctions c/ as funcoes que serao processadas no retorno do callback (Success)
function Set_jfunctions()
{
    jfunctions.push("tema = json_parsed.tema")
    jfunctions.push("cmips = json_parsed.cmip")
    jfunctions.push("cmdis = json_parsed.cmdi")
    jfunctions.push("cmpi = json_parsed.cmpi")
    jfunctions.push("cmtis = json_parsed.cmti")
    jfunctions.push("cmbis = json_parsed.cmbi")
    jfunctions.push("cmris = json_parsed.cmri")
    jfunctions.push("Execs(json_parsed.exec_first)")
    jfunctions.push("Set_Remove(json_parsed.remove)")
    jfunctions.push("Remove_dd(json_parsed.remove_dd)")
    jfunctions.push("Remove_linha(json_parsed.remove_linha)")
    jfunctions.push("Set_Hide(json_parsed.hide)")
    jfunctions.push("Pre_Layout(json_parsed.cmd)")
    jfunctions.push("trOptions_Check(json_parsed.trOptions)")
    jfunctions.push("Layout_Buttons(json_parsed.cmd_btn)")
    jfunctions.push("Layout_Spans(json_parsed.cmd_span)")
    jfunctions.push("Layout_Imagens(json_parsed.cmd_img)")
    jfunctions.push("Set_AutoCompl(json_parsed.autocompl)")
    jfunctions.push("Set_AutoCompl_Enable(json_parsed.autocompl_enable)")
    jfunctions.push("Set_Combo(json_parsed.combo)")
    jfunctions.push("Set_Values(json_parsed.value)")
    jfunctions.push("Set_Text(json_parsed.text)")
    jfunctions.push("Set_Prop(json_parsed.prop)")
    jfunctions.push("Set_Css(json_parsed.css)")
    jfunctions.push("Set_TabIndex(json_parsed.tabindex)")
    jfunctions.push("Set_Next_Focus(json_parsed.next_focus, (json_parsed.newtags != undefined))")
    jfunctions.push("Set_NewTags(json_parsed.newtags)")
    jfunctions.push("Box_effects()")
    jfunctions.push("localStorage.setItem('token', json_parsed.token)")
    jfunctions.push("Execs(json_parsed.exec)")
    jfunctions.push("Htbl_Resize_Columns_Check(json_parsed.cmd)")
    jfunctions.push("Show_Msg(json_parsed.msg)")
    jfunctions.push("Show_YesNoCancel(json_parsed.yesno)")
    jfunctions.push("Extra(json_parsed.extra)")
}

// Prepara os dados para serem enviados via callback para o Servidor e chama Perform_CallBack
function Collect_Params(caller_id, caller_event, send_token, preloader)
{
    var Ids = "id;e"
    var Values = caller_id + ";" + caller_event

    $(box_selector).not(":checkbox").each(function ()
    {
        Ids += ";" + this.id
        Values += ";" + convert_to_csv($(this).val())
    });

    $(":checkbox").each(function ()
    {
        Ids += ";" + this.id
        Values += ";" + $(this).prop("checked")
    });

    if (send_token)
    {
        Ids += ";token"
        Values += ";" + Get_Token()
    }

    // Envia as informações (id/value) das tags input, select, textarea e checkbox, o elemento relacionado ao focus e o caller_event (enter, tab, focus e etc.), o token e informacao p/ ativar ou nao o preloader.
    Perform_CallBack(Get_PageName() + "/Get_Elementos", "{'Ids':'" + Ids + "','Vls':'" + Values + "'}", preloader, caller_id, caller_event)
}

// Prepara o dado de um unico elemento para ser enviado via callback para o Servidor e chama Perform_CallBack
// Nao envia token e define preloader com false.
function Collect_Param_Element(caller, caller_event, send_token, preloader)
{
    var caller_id = caller.attr('id')
    var Ids = "id;e;" + caller_id
    var Values = caller_id + ";" + caller_event

    Values += ";" + convert_to_csv(caller.is(':checkbox') ? caller.prop('checked') : caller.val())

    if (send_token)
    {
        Ids += ";token"
        Values += ";" + Get_Token()
    }

    // Envia as informações (id/value) das tag input, select, textarea e checkbox, o elemento relacionado ao focus e o caller_event (enter, tab, focus e etc.), nesta function, nao envia token e define preloader com false.
    Perform_CallBack(Get_PageName() + "/Get_Elementos", "{'Ids':'" + Ids + "','Vls':'" + Values + "'}", preloader, caller_id, caller_event)
}

// Prepara o dado dos elementos, enviando a ordem dos elementos, apos evento updade do sortable para serem enviados via callback para o Servidor e chama Perform_CallBack
// Nao envia token e define preloader com false.
function Collect_Params_Order(caller_id, caller_event, div)
{
    var order = 1
    var Ids = "id;e"
    var Values = caller_id + ";" + caller_event

    $("#" + div + " span").each(function ()
    {
        Ids += ";" + this.id
        Values += ";" + order.toString()
        order += 1
    });

    // Envia as informações (id/value) das tags span, o elemento relacionado ao drag do sortable e o caller_event.
    Perform_CallBack(Get_PageName() + "/Get_Elementos", "{'Ids':'" + Ids + "','Vls':'" + Values + "'}", false, caller_id, caller_event)
}

// Efetua chamada p/ Collect_Params, usando setTimeout de 100ms, p/ exibir o  preview,
// ja que ao efetuar a chamada de Success_CallBack o preview nao e' exibido.
function xlImport(wb_name)
{
    setTimeout(function ()
    {
        Collect_Params('ExcelImportButton', wb_name, true, true);
    }, 100);
}

//  Efetua o Ajax CallBack para o Servidor
function Perform_CallBack(urlText, jsonText, preloader, caller_id, caller_event)
{
    $.ajax({
        url: urlText,
        data: jsonText,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (msg) { Success_CallBack(JSON.parse(msg.d), this.url, caller_id, caller_event) },
        error: function (jqXHR, status, error) { Error_CallBack(jqXHR, status, error) },
        beforeSend: function (jqXHR) { if (preloader) { $("#preloader").addClass("windows8").show() } },
        complete: function (jqXHR) { if (preloader) { $("#preloader").removeClass("windows8").hide() } }
    });
}

// Efetua o Ajax CallBack para processar funcoes associadas ao autocomplete.
function Set_Autocomplete(caller_id, urlText)
{
    $("#" + caller_id).autocomplete({
        minLength: 1,
        source: function (request, response)
        {
            $.ajax({
                url: urlText,
                data: "{ 'prefix':'" + request.term.replace("'", "") + "','focus_element':'" + caller_id + "' }",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data)
                {
                    response($.map(JSON.parse(data.d), function (item)
                    {
                        return { label: item.name, value: item.id, tipo: item.tipo, descr: item.descr }
                    }))
                },
                error: function (jqXHR, status, error) { Error_CallBack(jqXHR, status, error) }
            });
        },
        open: function (event, ui)
        {
            $("#tipo").val("")
            $("#codigo").val("")
        },
        focus: function (event, ui)
        {
            $("#" + caller_id).val(ui.item.label);
            return false; // Prevent the widget from inserting the value.
        },
        select: function (event, ui)
        {
            $("#tipo").val(ui.item.tipo)
            $("#codigo").val(ui.item.value)
            $("#" + caller_id).val(ui.item.label)
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item)
    {
        return $("<li>")
            .append("<a>" + item.label + ((item.descr) ? "<br>" + item.descr : "") + "</a>")
            .appendTo(ul);
    };
}

//  Efetua o Ajax CallBack p/ Upload_ImgFiles para o Servidor
function UploadFile(file, isImage, isLast)
{
    var isText = (checkfile_extension(file.name.toLowerCase(), new Array(".csv")))
    var fr = new FileReader()
    var chunkSize = isMobile() ? 524288 : 524288 * 2
    var vslice = 0
    var vparcial = 0

    function loadNext()
    {
        var start, end
        var status = ""

        vslice = (file.size - vparcial >= chunkSize) ? chunkSize : file.size - vparcial
        vparcial += vslice
        start = vparcial - vslice
        end = vparcial - 1

        if (vparcial == file.size && file.size <= chunkSize) { status = "start_end" }
        else if (vparcial == file.size) { status = "end" }
        else if (vparcial == chunkSize) { status = "start" }

        fr.onloadend = function (e)
        {
            var header, base64String, jsonText
            // a variavel header e' usada p/ remover o header gerado pelo FileReader (modo DataURL) com slice,
            // sem slice e' necessario acrescentar o file.type ("data:" + file.type + ";base64,")
            // No entanto o IE usa "data:" + file.type + ";base64,", portanto e' necessario checar o arquivo para
            // saber qual e' a informacao do header.

            if (isText)
            {
                base64String = e.target.result
            }
            else
            {
                header = "data:;base64,"

                if (e.target.result.indexOf("data:" + file.type + ";base64,") >= 0)
                {
                    header = "data:" + file.type + ";base64,"
                }
                else if (e.target.result.indexOf("data:application/octet-stream;base64,") >= 0)
                {
                    header = "data:application/octet-stream;base64,"
                }
                base64String = e.target.result.substring((isText) ? 0 : header.length)
            }

            jsonText = "{ 'base64String':'" + base64String + "','filename':'" + file.name + "','status':'" + status + "','isLast':'" + isLast + "','token':'" + Get_Token() + "'}"

            $.ajax({
                url: Get_PageName() + "/Upload_" + (isImage ? "ImgFiles" : "xlFiles"),
                data: jsonText,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (msg)
                {
                    var json_parsed = JSON.parse(msg.d)
                    Success_CallBack(json_parsed, this.url, "xl", "xl")

                    // Se ainda houver dados para serem enviados e nao houver o retorno de alguma 
                    // mensagem de inconsistencia ou alerta, chama novamente loadNext.
                    if (vparcial < file.size) { if (!json_parsed.msg.length) { loadNext() } }
                },
                error: function (jqXHR, status, error) { Error_CallBack(jqXHR, status, error) },
                beforeSend: function (jqXHR) { $("#preloader").addClass("windows8").show() },
                complete: function (jqXHR) { $("#preloader").removeClass("windows8").hide() }
            });
        };

        if (isText)
        {
            fr.readAsText(file.slice(start, end + 1), 'CP1252');
        }
        else
        {
            fr.readAsDataURL(file.slice(start, end + 1));
        }
    }
    loadNext();
}

// Processa as informações recebidas do Servidor
function Success_CallBack(json_parsed, urlText, caller_id, caller_event)
{
    // Caso nao tenha mensagens e caller_event = "enter" ou etc., seta msg_check p/ fechar o painel de mensagem (caso esteja aberto)
    var msg_check = ((caller_event != undefined && caller_event == "enter") || (caller_id != undefined && caller_id.endsWith("Remover")))

    if (json_parsed.jfunctions != undefined)
    {
        for (var i = 0; i <= json_parsed.jfunctions.length - 1; i++)
        {
            eval(jfunctions[json_parsed.jfunctions[i]])
        }

        if (json_parsed.msg == undefined && msg_check) { $("#msgPanel_" + caller_id).remove() }
    }
    else
    {
        if (msg_check) { $("#msgPanel_" + caller_id).remove() }
    }
}

// Conforme os parametros exibe mensagens de erro do callback chamando Show_Msg
function Error_CallBack(jqXHR, status, error)
{
    var msg = "Falha detectada ao efetuar comunicacao com o servidor.<br>"

    if (jqXHR.status === 0) { msg += "Sem conexao. Verifique a rede.<br>" }
    else if (jqXHR.status == 400) { msg += "400 - O conteudo da solicitacao nao e' valido.<br>" }
    else if (jqXHR.status == 401) { msg += "401 - Acesso nao autorizado.<br>" }
    else if (jqXHR.status == 403) { msg += "403 - Recurso foi perdido e nao pode mais ser acessado.<br>" }
    else if (jqXHR.status == 404) { msg += "404 - Pagina solicitada nao encontrada.<br>" }
    else if (jqXHR.status == 500) { msg += "500 - Erro interno do servidor.<br>" }
    else if (jqXHR.status == 503) { msg += "503 - Servico Indisponivel" }
    else if (status === 'parsererror') { msg += "Falha ao processar arquivo JSON.<br>" }
    else if (status === 'timeout') { msg += "Tempo esgotado.<br>" }
    else if (status === 'abort') { msg += "Solicitacao Ajax interrompida.<br>" }

    var rtxt = jQuery.parseJSON(jqXHR.responseText);
    msg += rtxt.Message + "<br>"
    // msg += "StackTrace: " + rtxt.StackTrace
    // msg += "ExceptionType: " + rtxt.ExceptionType

    msg = '[{"msg":\"' + msg + '\","id":"MessagesPanel","panelMsg":true,"tag_alert":false,"green":false,"blue":false,"close":false}]'

    Show_Msg(JSON.parse(msg))
    Set_Next_Focus("MessagesPanel")
}

// Checa e processa algumas informacoes ocasionais (reload page, previous page, new page, qs, round_table)
function Extra(extra)
{
    var item
    for (var i = 0; i <= extra.length - 1; i++)
    {
        item = extra[i]

        // Chama novamente Callback p/ restaurar as informacoes da session do usuario.
        if (item.comeback) { Collect_Params("Start", "Start_ComeBack", true, true) }

        // Grava a informacao em localStorage
        if (item.qsvalue) { localStorage.setItem("QS_" + item.qspagina, item.qsvalue) }

        // Reload page
        if (item.reload) { parent.location.reload(true) }

        // Redireciona para a pagina anterior
        if (item.prevurl)
        {
            if (localStorage.getItem("previous_page"))
            {
                previous_page = localStorage.getItem("previous_page")
                localStorage.removeItem("previous_page")
                parent.window.location = previous_page
            }
        }

        // Redireciona a pagina para a url informada.
        if (item.newurl)
        {
            localStorage.setItem("previous_page", Get_PageName())
            parent.window.location = item.newurl
        }

        // Chama novamente Callback p/ inicializar os elementos com os parametros informados em localStorage ao inves de querystring
        if (item.qs)
        {
            if (localStorage.getItem("QS_" + Get_PageName()))
            {
                var qs_elements = JSON.stringify(localStorage.getItem("QS_" + Get_PageName()))

                localStorage.removeItem("QS_" + Get_PageName())

                Perform_CallBack(Get_PageName() + "/Get_CadQS", "{'qs_elements':" + qs_elements + ",'token':'" + Get_Token() + "'}", true, "Start", "Start_QS")
            }
        }
    }
}

