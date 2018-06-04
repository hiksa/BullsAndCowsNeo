let game = new Phaser.Game(800, 600, Phaser.AUTO, 'game');

game.state.add('titleState', titleState);
game.state.add('queueState', queueState);
game.state.add('gameState', gameState);

game.state.start('titleState');