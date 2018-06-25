var queueState = {
    preload: function () {
        game.load.image('sky', 'images/sky.png');
    },

    create: function () {
        game.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;

        game.scale.pageAlignHorizontally = true;
        game.scale.pageAlignVertically = true;

        game.background = game.add.sprite(0, 0, 'sky');

        // TODO: Find another player and transition to next state
        game.input.onTap.addOnce(transitionToGameState, this);

        var text = "- Waiting for other player -";
        var style = { font: "40px Arial", fill: "#ff0044", align: "center" };

        game.add.text(game.world.centerX - 220, 250, text, style);
    },

    update: function () {

    }
};

function transitionToGameState() {
    game.state.start('gameState');
}