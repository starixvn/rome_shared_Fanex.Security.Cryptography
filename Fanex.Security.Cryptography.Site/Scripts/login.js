$(document).ready(function () {
    changePwd();
    $('#secret').on('keyup,cut,paste', function () {
        changePwd();
    });

    $('#plainText').on('keyup,cut,paste', function () {
        changePwd();
    });
});

function encrypt(plainText, secretKey) {
    var passPhrase = 'a5s8d2e9c172';
    var key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
    var iv = key;
    var pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
    {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    return pwd;
};

function decrypt(encrypted, secretKey) {
    try {
        var passPhrase = 'a5s8d2e9c172';
        var key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
        var iv = key;

        var pwd = CryptoJS.AES.decrypt(encrypted, key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }).toString(CryptoJS.enc.Utf8);

        return pwd;
    } catch (e) {
        return 'Error';
    }
};

function changePwd() {
    var plainText = $('#plainText').val();
    var secret = $('#secret').val();

    var en = encrypt(plainText, secret);
    var de = decrypt(en, secret);

    $('#encrypted').val(en);
    $('#decrypted').val(de);
};