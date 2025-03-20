function AtualizaListaBeneficiarios(beneficiarios) {
    var tbody = document.querySelector('#tblBeneficiarios tbody');

    tbody.innerHTML = ''; // Limpa o conteúdo da tabela

    beneficiarios.forEach((beneficiario, i) => {
        var row = `
        <tr>
        <td>${beneficiario.Cpf}</td>
        <td>${beneficiario.Nome}</td>
        <td>
            <button class="btn btn-primary" onclick="AlterarBeneficiario(${i},'${beneficiario.Nome}','${beneficiario.Cpf}')">Alterar</button>
            <button class="btn btn-primary" onclick="ExcluirBeneficiario(${i})">Excluir</button>
        </td>
    </tr>`;
        tbody.innerHTML += row;
    })
}

function ExcluirBeneficiario(id) {
    $.ajax({
        url: urlDeleteBeneficiario,  // URL para o método do controlador
        type: 'POST',  // Método de solicitação (POST ou GET)
        data: { Id: id },  // Dados enviados para o servidor
        success: function (response) {
            // Manipule a resposta do servidor
            AtualizaListaBeneficiarios(response.Data)
        },
        error: function (xhr, status, error) {
            // Manipule qualquer erro que ocorra durante a solicitação
            ModalDialog("Ocorreu um erro", error);
        }
    });
}

function AlterarBeneficiario(id, nome, cpf) {
    $("tbody button").prop('disabled', true);
    $("#BtnAlterarBeneficiario").removeClass("hidden");
    $("#BtnCancelarBeneficiario").removeClass("hidden");
    $("#BtnIncluirBeneficiario").addClass("hidden");
    $("#NomeBeneficiario").val(nome);
    $("#CpfBeneficiario").val(cpf);
    $("#IndexBeneficiario").val(id);
}
function resetModalBeneficiarios() {
    $("#formCadastroBeneficiarios")[0].reset();
    $("tbody button").prop('disabled', false);
    $("#BtnAlterarBeneficiario").addClass("hidden");
    $("#BtnCancelarBeneficiario").addClass("hidden");
    $("#BtnIncluirBeneficiario").removeClass("hidden");
}

$(document).ready(function () {

    var tbody = document.querySelector('#tblBeneficiarios tbody');

    $('#BtnModalBeneficiarios').click(() => {
        $('#ModalBeneficiarios').modal('show');
    });

    $('#BtnFecharModalBeneficiarios').click(() => {
        $('#ModalBeneficiarios').modal('hide');
        resetModalBeneficiarios();
    });

    $("#Cpf").inputmask("mask", { "mask": "999.999.999-99" });
    $("#CpfBeneficiario").inputmask("mask", { "mask": "999.999.999-99" });
    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "Cpf": $(this).find("#Cpf").val(),
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500) {
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    }
                },
            success:
                function (r) {
                    tbody.innerHTML = '';
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });
    })

    $('#formCadastroBeneficiarios').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: urlPostBeneficiario,
            method: "POST",
            data: {
                "Nome": $(this).find("#NomeBeneficiario").val(),
                "Cpf": $(this).find("#CpfBeneficiario").val()
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    const beneficiarios = r.Data;
                    AtualizaListaBeneficiarios(beneficiarios);

                    $("#formCadastroBeneficiarios")[0].reset();
                }
        });
    })

    $('#BtnAlterarBeneficiario').click(function (e) {

        e.preventDefault();
        $.ajax({
            url: urlPutBeneficiario,
            method: "POST",
            data: {
                "model": {
                    "Nome": $("#NomeBeneficiario").val(),
                    "Cpf": $("#CpfBeneficiario").val()
                },
                "Index": $("#IndexBeneficiario").val(),
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    const beneficiarios = r.Data;
                    AtualizaListaBeneficiarios(beneficiarios);
                    resetModalBeneficiarios();
                }
        });


    })


    $('#BtnCancelarBeneficiario').click((e) => {
        e.preventDefault();
        resetModalBeneficiarios();
    })

})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
