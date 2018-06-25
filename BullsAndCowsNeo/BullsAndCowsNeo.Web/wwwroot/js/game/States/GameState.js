var gameState = {
    preload: function () {
        game.load.image('background', 'images/background.png');
        game.load.spritesheet('button', 'images/button.png', 193, 71);
    },

    create: function () {
        game.background = game.add.sprite(0, 0, 'background');
        game.add.plugin(PhaserInput.Plugin);
        game.inputField = game.add.inputField(330, 250, { height: 14});

        game.inputButton = game.add.button(game.world.centerX - 95, 400, 'button', actionOnClick, this, 0);
    },

    update: function () {

    }
};

function actionOnClick() {
    // TODO: Secret number enter
    console.log('button clicked');
}