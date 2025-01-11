using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class InstrumentPresenterTests
    {
        private Mock<IStorageDataBase<Instrument>> _mockInstrumentStorage;
        private InstrumentPresenter _instrumentPresenter;

        [SetUp]
        public void SetUp()
        {
            _mockInstrumentStorage = new Mock<IStorageDataBase<Instrument>>();
            _instrumentPresenter = new InstrumentPresenter(_mockInstrumentStorage.Object);
        }

        // Тест для AddInstrumentAsync
        [Test]
        public async Task AddInstrumentAsync_ShouldAddInstrument()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var instrumentName = "Guitar";
            var token = CancellationToken.None;

            // Act
            await _instrumentPresenter.AddInstrumentAsync(instrumentId, instrumentName, token);

            // Assert
            _mockInstrumentStorage.Verify(storage => storage.AddAsync(It.Is<Instrument>(i => i.Id == instrumentId && i.Name == instrumentName), token), Times.Once);
        }

        // Тест для DeleteInstrumentAsync
        [Test]
        public async Task DeleteInstrumentAsync_ShouldDeleteInstrument()
        {
            // Arrange
            var instrument = new Instrument(Guid.NewGuid(), "Guitar");
            var token = CancellationToken.None;

            // Act
            await _instrumentPresenter.DeleteInstrumentAsync(instrument, token);

            // Assert
            _mockInstrumentStorage.Verify(storage => storage.DeleteAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token), Times.Once);
        }

        // Тест для GetInstrumentsAsync
        [Test]
        public async Task GetInstrumentsAsync_ShouldReturnInstruments()
        {
            // Arrange
            var token = CancellationToken.None;
            var instruments = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Guitar"),
                new Instrument(Guid.NewGuid(), "Piano")
            };

            _mockInstrumentStorage.Setup(storage => storage.GetListAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token))
                .ReturnsAsync(instruments);

            // Act
            var result = await _instrumentPresenter.GetInstrumentsAsync(token);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(instruments[0], result.ToList());
            Assert.Contains(instruments[1], result.ToList());
        }

        // Тест для GetInstrumentByIdAsync
        [Test]
        public async Task GetInstrumentByIdAsync_ShouldReturnInstrumentById()
        {
            // Arrange
            var instrumentId = Guid.NewGuid();
            var instrument = new Instrument(instrumentId, "Guitar");
            var token = CancellationToken.None;

            _mockInstrumentStorage.Setup(storage => storage.GetSingleAsync(It.IsAny<Func<IQueryable<Instrument>, IQueryable<Instrument>>>(), token))
                .ReturnsAsync(instrument);

            // Act
            var result = await _instrumentPresenter.GetInstrumentByIdAsync(instrumentId, token);

            // Assert
            Assert.AreEqual(instrumentId, result.Id);
            Assert.AreEqual("Guitar", result.Name);
        }
    }
}
