using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianPresenterTests
    {
        private Mock<IMusicianStorage> _mockMusicianStorage;
        private Mock<InstrumentPresenter> _mockInstrumentPresenter;
        private Mock<MusicianInstrumentPresenter> _mockMusicianInstrumentPresenter;
        private MusicianPresenter _musicianPresenter;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _mockMusicianStorage = new Mock<IMusicianStorage>();
            _mockInstrumentPresenter = new Mock<InstrumentPresenter>();
            _mockMusicianInstrumentPresenter = new Mock<MusicianInstrumentPresenter>();
            _musicianPresenter = new MusicianPresenter(_mockMusicianStorage.Object)
            {
                // Подмена приватных зависимостей
                _instrumentPresenter = _mockInstrumentPresenter.Object,
                _musicianInstrumentPresenter = _mockMusicianInstrumentPresenter.Object
            };
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task AddMusicianAsync_ShouldAddMusicianAndAssociateInstruments()
        {
            // Arrange
            var name = "John";
            var lastName = "Doe";
            var surname = "Smith";
            var instruments = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Guitar"),
                new Instrument(Guid.NewGuid(), "Piano")
            };

            _mockMusicianStorage
                .Setup(storage => storage.AddMusicianAsync(It.IsAny<Musician>(), _cancellationToken))
                .Returns(Task.CompletedTask);

            _mockInstrumentPresenter
                .Setup(presenter => presenter.AddInstrumentAsync(It.IsAny<Guid>(), It.IsAny<string>(), _cancellationToken))
                .Returns(Task.CompletedTask);

            _mockMusicianInstrumentPresenter
                .Setup(presenter => presenter.AddMusicianInstrumentAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), _cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _musicianPresenter.AddMusicianAsync(name, lastName, surname, instruments, _cancellationToken);

            // Assert
            _mockMusicianStorage.Verify(
                storage => storage.AddMusicianAsync(It.Is<Musician>(m => m.Name == name && m.LastName == lastName && m.Surname == surname), _cancellationToken),
                Times.Once);

            foreach (var instrument in instruments)
            {
                _mockInstrumentPresenter.Verify(
                    presenter => presenter.AddInstrumentAsync(instrument.Id, instrument.Name, _cancellationToken),
                    Times.Once);

                _mockMusicianInstrumentPresenter.Verify(
                    presenter => presenter.AddMusicianInstrumentAsync(It.IsAny<Guid>(), instrument.Id, _cancellationToken),
                    Times.Once);
            }

            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(lastName, result.LastName);
            Assert.AreEqual(surname, result.Surname);
        }

        [Test]
        public async Task DeleteMusicianAsync_ShouldCallDeleteMusicianAsync()
        {
            // Arrange
            var musician = new Musician(Guid.NewGuid(), "John", "Doe", "Smith");

            _mockMusicianStorage
                .Setup(storage => storage.DeleteMusicianAsync(musician, _cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _musicianPresenter.DeleteMusicianAsync(musician, _cancellationToken);

            // Assert
            _mockMusicianStorage.Verify(storage => storage.DeleteMusicianAsync(musician, _cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetMusiciansAsync_ShouldReturnAllMusicians()
        {
            // Arrange
            var musicians = new List<Musician>
            {
                new Musician(Guid.NewGuid(), "John", "Doe", "Smith"),
                new Musician(Guid.NewGuid(), "Jane", "Doe", "Smith")
            };

            _mockMusicianStorage
                .Setup(storage => storage.GetAllMusiciansAsync(_cancellationToken))
                .ReturnsAsync(musicians);

            // Act
            var result = await _musicianPresenter.GetMusiciansAsync(_cancellationToken);

            // Assert
            Assert.AreEqual(musicians, result);
            _mockMusicianStorage.Verify(storage => storage.GetAllMusiciansAsync(_cancellationToken), Times.Once);
        }
    }
}
