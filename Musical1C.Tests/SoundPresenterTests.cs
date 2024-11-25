using System.Diagnostics.CodeAnalysis;
using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SoundPresenterTests
    {
        private Mock<IMusicStorage> _mockMusicStorage;
        private SoundPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _mockMusicStorage = new Mock<IMusicStorage>();
            _presenter = new SoundPresenter(_mockMusicStorage.Object);
        }

        [Test]
        public async Task AddMusicAsync_ShouldCallAddMusicAsync()
        {
            // Arrange
            var name = "Symphony No. 5";
            var author = "Beethoven";
            var token = CancellationToken.None;

            // Act
            var music = await _presenter.AddMusicAsync(name, author, token);

            // Assert
            _mockMusicStorage.Verify(s => s.AddMusicAsync(It.Is<Sound>(m => m.Name == name && m.Author == author), token), Times.Once);
            Assert.IsNotNull(music);
            Assert.AreEqual(name, music.Name);
            Assert.AreEqual(author, music.Author);
        }

        [Test]
        public async Task DeleteMusicAsync_ShouldCallDeleteMusicAsync()
        {
            // Arrange
            var music = new Sound(Guid.NewGuid(), "Moonlight Sonata", "Beethoven");
            var token = CancellationToken.None;

            // Act
            await _presenter.DeleteMusicAsync(music, token);

            // Assert
            _mockMusicStorage.Verify(s => s.DeleteMusicAsync(music, token), Times.Once);
        }

        [Test]
        public async Task GetMusicAsync_ShouldReturnListOfMusic()
        {
            // Arrange
            var expectedMusic = new List<Sound>
            {
                new Sound(Guid.NewGuid(), "Symphony No. 5", "Beethoven"),
                new Sound(Guid.NewGuid(), "Clair de Lune", "Debussy")
            };
            
            var token = CancellationToken.None;

            _mockMusicStorage.Setup(s => s.GetAllMusicAsync(token))
                .ReturnsAsync(expectedMusic);

            // Act
            var result = await _presenter.GetMusicAsync(token);

            // Assert
            Assert.AreEqual(expectedMusic.Count, result.Count);
            CollectionAssert.AreEquivalent(expectedMusic, result);
        }
    }
}