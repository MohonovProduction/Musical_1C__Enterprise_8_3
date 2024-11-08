using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianPresenterTests
    {
        private Mock<IMusicianStorage> _mockMusicianStorage;
        private MusicianPresenter _musicianPresenter;
        private CancellationToken _cancellationToken;
        [SetUp]
        public void SetUp()
        {
            _mockMusicianStorage = new Mock<IMusicianStorage>();
            _musicianPresenter = new MusicianPresenter(_mockMusicianStorage.Object);
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task AddMusicianAsync_ShouldCallAddMusicianAsyncOnStorage()
        {
            // Arrange
            var name = "John";
            var lastName = "Doe";
            var surname = "Smith";
            var instruments = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Guitar")
            };
            var cancellationToken = _cancellationToken;

            // Act
            var musician = await _musicianPresenter.AddMusicianAsync(name, lastName, surname, instruments, cancellationToken);

            // Assert
            _mockMusicianStorage.Verify(
                storage => storage.AddMusicianAsync(It.Is<Musician>(m =>
                    m.Name == name &&
                    m.LastName == lastName &&
                    m.Surname == surname &&
                    m.Instruments == instruments
                ), cancellationToken),
                Times.Once
            );
            Assert.AreEqual(name, musician.Name);
            Assert.AreEqual(lastName, musician.LastName);
            Assert.AreEqual(surname, musician.Surname);
            Assert.AreEqual(instruments, musician.Instruments);
        }

        [Test]
        public async Task DeleteMusicianAsync_ShouldCallDeleteMusicianAsyncOnStorage()
        {
            // Arrange
            var musician = new Musician(Guid.NewGuid(), "Jane", "Doe", "Smith", new List<Instrument>());
            var cancellationToken = _cancellationToken;

            // Act
            await _musicianPresenter.DeleteMusicianAsync(musician, cancellationToken);

            // Assert
            _mockMusicianStorage.Verify(
                storage => storage.DeleteMusicianAsync(musician, cancellationToken),
                Times.Once
            );
        }

        [Test]
        public async Task GetMusiciansAsync_ShouldReturnMusiciansFromStorage()
        {
            // Arrange
            var cancellationToken =  _cancellationToken;
            var expectedMusicians = new List<Musician>
            {
                new Musician(Guid.NewGuid(), "Alice", "Doe", "Smith", new List<Instrument>()),
                new Musician(Guid.NewGuid(), "Bob", "Johnson", "Lee", new List<Instrument>())
            };
            _mockMusicianStorage.Setup(storage => storage.GetAllMusiciansAsync(cancellationToken))
                                .ReturnsAsync(expectedMusicians);

            // Act
            var musicians = await _musicianPresenter.GetMusiciansAsync(cancellationToken);

            // Assert
            Assert.AreEqual(expectedMusicians, musicians);
        }
    }
}
