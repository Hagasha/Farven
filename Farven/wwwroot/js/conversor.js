$(document).ready(function () {
    // Carregar as moedas disponíveis
    $.ajax({
        url: "https://api.frankfurter.app/currencies",
        success: function (data) {
            $.each(data, function (code, name) {
                $("#from, #to").append($("<option>", {
                    value: code,
                    text: `${code} - ${name}`
                }));
            });
        },
        error: function () {
            $("#result").html("Erro ao carregar as moedas.");
        }
    });
});

function convertCurrency() {
    var fromCurrency = $("#from").val();
    var toCurrency = $("#to").val();
    var amount = $("#amount").val();

    if (fromCurrency && toCurrency && amount) {
        $.ajax({
            url: `https://api.frankfurter.app/latest?base=${fromCurrency}&symbols=${toCurrency}`,
            success: function (data) {
                const rate = data.rates[toCurrency];
                const convertedAmount = (amount * rate).toFixed(2);
                $("#result").html(`${amount} ${fromCurrency} = ${convertedAmount} ${toCurrency}`);

                // Enviar dados de conversão para o backend
                $.ajax({
                    url: '/api/conversionhistory',
                    method: 'POST',
                    data: JSON.stringify({
                        fromCurrency: fromCurrency,
                        toCurrency: toCurrency,
                        amount: parseFloat(amount), // Certifique-se de enviar como número
                        convertedAmount: parseFloat(convertedAmount) // Certifique-se de enviar como número
                    }),
                    contentType: 'application/json',
                    success: function () {
                        console.log('Histórico de conversão salvo com sucesso.');
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.error(`Erro ao salvar o histórico de conversão: ${textStatus} - ${errorThrown}`);
                        console.error('Detalhes da resposta:', jqXHR.responseText); // Para ver a resposta do servidor
                    }
                });
            },
            error: function () {
                $("#result").html("Ocorreu um erro na conversão.");
            }
        });
    } else {
        $("#result").html("Por favor, preencha todos os campos.");
    }
}