var titleState = {
    preload: function () {
        game.load.image('sky', 'images/sky.png');
    },

    create: function () {
        game.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
        console.log()
        game.scale.pageAlignHorizontally = true;
        game.scale.pageAlignVertically = true;

        game.background = game.add.sprite(0, 0, 'sky');
        game.input.onTap.addOnce(transitionToQueueState, this);

        var text = "- Click to enter queue -";
        var style = { font: "40px Arial", fill: "#ff0044", align: "center" };
        game.add.text(game.world.centerX - 220, 250, text, style);
    },

    update: function () {

    }
};

function transitionToQueueState() {
    game.state.start('queueState');
}