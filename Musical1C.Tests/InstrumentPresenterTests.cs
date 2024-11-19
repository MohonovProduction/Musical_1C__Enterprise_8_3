using System.Diagnostics.CodeAnalysis;
using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class InstrumentPresenterTests
    {
        private Mock<IInstrumentStorage> _mockInstrumentStorage;
        private InstrumentPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _mockInstrumentStorage = new Mock<IInstrumentStorage>();
            _presenter = new InstrumentPresenter(_mockInstrumentStorage.Object);
        }

        [Test]
        public async Task AddInstrumentAsync_ShouldCallAddInstrumentAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Guitar";
            var token = CancellationToken.None;

            // Act
            await _presenter.AddInstrumentAsync(id, name, token);

            // Assert
            _mockInstrumentStorage.Verify(s => s.AddInstrumentAsync(It.Is<Instrument>(i => i.Id == id && i.Name == name), token), Times.Once);
        }

        [Test]
        public async Task DeleteInstrumentAsync_ShouldCallDeleteInstrumentAsync()
        {
            // Arrange
            var instrument = new Instrument(Guid.NewGuid(), "Piano");
            var token = CancellationToken.None;

            // Act
            await _presenter.DeleteInstrumentAsync(instrument, token);

            // Assert
            _mockInstrumentStorage.Verify(s => s.DeleteInstrumentAsync(instrument, token), Times.Once);
        }

        [Test]
        public async Task GetInstrumentsAsync_ShouldReturnInstruments()
        {
            // Arrange
            var expectedInstruments = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Guitar"),
                new Instrument(Guid.NewGuid(), "Piano")
            };
            var token = CancellationToken.None;

            _mockInstrumentStorage.Setup(s => s.GetInstrumentsAsync(token))
                .ReturnsAsync(expectedInstruments);

            // Act
            var result = await _presenter.GetInstrumentsAsync(token);

            // Assert
            Assert.AreEqual(expectedInstruments.Count, result.Count);
            CollectionAssert.AreEquivalent(expectedInstruments, result);
        }
    }
}