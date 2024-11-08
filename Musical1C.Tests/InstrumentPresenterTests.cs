using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class InstrumentPresenterTests
    {
        private Mock<IInstrumentStorage> _mockInstrumentStorage;
        private InstrumentPresenter _presenter;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _mockInstrumentStorage = new Mock<IInstrumentStorage>();
            _presenter = new InstrumentPresenter(_mockInstrumentStorage.Object);
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task AddInstrumentAsync_ShouldAddInstrument()
        {
            // Arrange
            var token = _cancellationToken;
            var instrumentName = "Guitar";

            // Act
            await _presenter.AddInstrumentAsync(instrumentName, token);

            // Assert
            _mockInstrumentStorage.Verify(storage => storage.AddInstrumentAsync(It.IsAny<Instrument>(), token), Times.Once);
        }

        [Test]
        public async Task DeleteInstrumentAsync_ShouldDeleteInstrument()
        {
            // Arrange
            var token = _cancellationToken;
            var instrumentId = Guid.NewGuid();
            var instrument = new Instrument(instrumentId, "Piano");

            // Act
            await _presenter.DeleteInstrumentAsync(instrument, token);

            // Assert
            _mockInstrumentStorage.Verify(storage => storage.DeleteInstrumentAsync(instrument, token), Times.Once);
        }

        [Test]
        public async Task GetInstrumentsAsync_ShouldReturnInstruments()
        {
            // Arrange
            var token = _cancellationToken;
            var instrumentsList = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Drum"),
                new Instrument(Guid.NewGuid(), "Violin")
            };

            _mockInstrumentStorage.Setup(storage => storage.GetInstrumentsAsync(token)).ReturnsAsync(instrumentsList);

            // Act
            var result = await _presenter.GetInstrumentsAsync(token);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.Contains(result.First(i => i.Name == "Drum"), result.ToList());
            Assert.Contains(result.First(i => i.Name == "Violin"), result.ToList());
        }
    }
}