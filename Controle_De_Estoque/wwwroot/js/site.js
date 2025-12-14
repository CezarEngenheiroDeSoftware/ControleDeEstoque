// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(document).on('click', '.btn-editar', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        $.ajax({
            type: "GET",
            url: `/EstoquePrincipal/Update?id=${id}`,
            success: function (html) {
                $('#conteudoModalEditar').html(html);
                $('#modalEdicao').modal('show');
            }
        });
    });
    $(document).on('submit', '#formUpdateEstoque', function (e) {
        e.preventDefault();
        if (!$("#produto_meli").length) {
            alert("Produto do Mercado Livre não encontrado no formulário.");
            return;
        }

            $.ajax({
                type: "POST",
                url: `/EstoquePrincipal/UpdateEstoque`,
                data: $(this).serialize(),
                beforeSend: function () {
                    $('#loadingSpinner').show();
                    $('#btnLoginSpinner').removeClass("d-none");
                },
                success: function (html) {
                    $('#loadingSpinner').hide();
                    if (html.success) {
                        alert("Estoque atualizado com sucesso!");
                        $('#modalEdicao').modal('hide');
                        location.reload();
                    } else {
                        /*alert(html.message);*/
                        $('#modalEdicao').modal('hide');
                        location.reload();
                    }
                },
                complete: function () {
                    $('#loadingSpinner').hide();
                    $("#btnLoginSpinner").addClass("d-none");
                    $('#modalEdicao').modal('hide');
                    location.reload();
                },
            error: function (e) {
                e.responseText;
            }
        });
    });
    console.log("js carregado");
    $(document).on('click', '.btn_add', function (e) {
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: "/EstoquePrincipal/CreateView",
            success: function (a) {
                $("#conteudoModalCreate").html(a);
                $("#modalCreate").modal("show");
            }
        });
    });
    $(document).on('submit', '#form-Create', function (e) {
        e.preventDefault();
        $("#txt").addClass("d-none");
        $("#btnLoginSpinner").removeClass("d-none");

        $.ajax({
            type: "POST",
            url: "/EstoquePrincipal/Create",
            data: $(this).serialize(),
            success: function (a) {
                $("#txt").removeClass("d-none");
                $("#btnLoginSpinner").addClass("d-none");
                $("#modalCreate").modal("hide");
                location.reload();
            }
        });
    });
});

