using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicPresenterTests
    {
        private Mock<IMusicStorage> _mockMusicStorage;
        private MusicPresenter _musicPresenter;

        [SetUp]
        public void SetUp()
        {
            _mockMusicStorage = new Mock<IMusicStorage>();
            _musicPresenter = new MusicPresenter(_mockMusicStorage.Object);
        }

        [Test]
        public async Task AddMusicAsync_ShouldReturnMusic_WhenCalled()
        {
            // Arrange
            var name = "Test Music";
            var author = "Test Author";
            var token = CancellationToken.None;
            var expectedMusic = new Music(Guid.NewGuid(), name, author);
            _mockMusicStorage.Setup(storage => storage.AddMusicAsync(It.IsAny<Music>(), token))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _musicPresenter.AddMusicAsync(name, author, token);

            // Assert
            Assert.That(result.Name, Is.EqualTo(expectedMusic.Name));
            Assert.That(result.Author, Is.EqualTo(expectedMusic.Author));
            _mockMusicStorage.Verify(storage => storage.AddMusicAsync(It.IsAny<Music>(), token), Times.Once);
        }

        [Test]
        public async Task DeleteMusicAsync_ShouldCallDeleteMusic_WhenCalled()
        {
            // Arrange
            var music = new Music(Guid.NewGuid(), "Test Music", "Test Author");
            var token = CancellationToken.None;
            _mockMusicStorage.Setup(storage => storage.DeleteMusicAsync(music, token))
                             .Returns(Task.CompletedTask);

            // Act
            await _musicPresenter.DeleteMusicAsync(music, token);

            // Assert
            _mockMusicStorage.Verify(storage => storage.DeleteMusicAsync(music, token), Times.Once);
        }

        [Test]
        public async Task GetMusicAsync_ShouldReturnMusicList_WhenCalled()
        {
            // Arrange
            var musicList = new List<Music>
            {
                new Music(Guid.NewGuid(), "Test Music 1", "Test Author 1"),
                new Music(Guid.NewGuid(), "Test Music 2", "Test Author 2")
            };
            var token = CancellationToken.None;
            _mockMusicStorage.Setup(storage => storage.GetAllMusicAsync(token))
                             .ReturnsAsync(musicList);

            // Act
            var result = await _musicPresenter.GetMusicAsync(token);

            // Assert
            Assert.That(result.Count, Is.EqualTo(musicList.Count));
            
            // Используем метод ElementAt для доступа по индексу
            Assert.That(result.ElementAt(0).Name, Is.EqualTo(musicList.ElementAt(0).Name));
            Assert.That(result.ElementAt(1).Name, Is.EqualTo(musicList.ElementAt(1).Name));
            
            _mockMusicStorage.Verify(storage => storage.GetAllMusicAsync(token), Times.Once);
        }

        [Test]
        public void Constructor_ShouldCreateMusicPresenter_WithMusicStorage()
        {
            // Arrange & Act
            var presenter = new MusicPresenter();

            // Assert
            Assert.IsNotNull(presenter);
        }
    }
}
