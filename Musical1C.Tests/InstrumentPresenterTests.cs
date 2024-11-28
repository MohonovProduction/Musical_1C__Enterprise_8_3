using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class InstrumentPresenterTests
    {
        private Mock<IStorageDataBase<Instrument>> _instrumentStorageMock;
        private InstrumentPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _instrumentStorageMock = new Mock<IStorageDataBase<Instrument>>();
            _presenter = new InstrumentPresenter(_instrumentStorageMock.Object);
        }

        [Test]
        public async Task AddInstrumentAsync_ShouldCallAddAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Guitar";
            var cancellationToken = CancellationToken.None;

            // Act
            await _presenter.AddInstrumentAsync(id, name, cancellationToken);

            // Assert
            _instrumentStorageMock.Verify(s => s.AddAsync(It.Is<Instrument>(
                i => i.Id == id && i.Name == name), cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteInstrumentAsync_ShouldCallDeleteAsync()
        {
            // Arrange
            var instrument = new Instrument(Guid.NewGuid(), "Drums");
            var cancellationToken = CancellationToken.None;

            // Act
            await _presenter.DeleteInstrumentAsync(instrument, cancellationToken);

            // Assert
            _instrumentStorageMock.Verify(s => s.DeleteAsync(
                It.Is<string>(where => where == $"Id = {instrument.Id}"),
                It.IsAny<object>(),
                cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetInstrumentsAsync_ShouldReturnInstruments()
        {
            // Arrange
            var instruments = new List<Instrument>
            {
                new Instrument(Guid.NewGuid(), "Piano"),
                new Instrument(Guid.NewGuid(), "Violin")
            };
            var cancellationToken = CancellationToken.None;

            _instrumentStorageMock.Setup(s => s.GetListAsync(
                It.Is<string>(where => where == null), 
                It.Is<object?>(parameters => parameters == null), 
                cancellationToken))
                .ReturnsAsync(instruments);

            // Act
            var result = await _presenter.GetInstrumentsAsync(cancellationToken);

            // Assert
            Assert.AreEqual(instruments, result);
            _instrumentStorageMock.Verify(s => s.GetListAsync(
                It.Is<string>(where => where == null), 
                It.Is<object?>(parameters => parameters == null), 
                cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetInstrumentByIdAsync_ShouldReturnCorrectInstrument()
        {
            // Arrange
            var id = Guid.NewGuid();
            var instrument = new Instrument(id, "Flute");
            var cancellationToken = CancellationToken.None;

            _instrumentStorageMock.Setup(s => s.GetSingleAsync(
                It.Is<string>(where => where == $"Id = {id}"),
                It.IsAny<object>(),
                cancellationToken))
                .ReturnsAsync(instrument);

            // Act
            var result = await _presenter.GetInstrumentByIdAsync(id, cancellationToken);

            // Assert
            Assert.AreEqual(instrument, result);
            _instrumentStorageMock.Verify(s => s.GetSingleAsync(
                It.Is<string>(where => where == $"Id = {id}"),
                It.IsAny<object>(),
                cancellationToken), Times.Once);
        }
    }
}
