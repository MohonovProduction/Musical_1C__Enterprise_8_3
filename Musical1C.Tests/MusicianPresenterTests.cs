using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests;

[TestFixture]
public class MusicianPresenterTests
{
    private Mock<IStorageDataBase<Musician>> _mockMusicianStorage;
    private Mock<IStorageDataBase<Instrument>> _mockInstrumentStorage;
    private Mock<IStorageDataBase<MusicianInstrument>> _mockMusicianInstrumentStorage;
    private MusicianPresenter _presenter;

    [SetUp]
    public void Setup()
    {
        _mockMusicianStorage = new Mock<IStorageDataBase<Musician>>();
        _mockInstrumentStorage = new Mock<IStorageDataBase<Instrument>>();
        _mockMusicianInstrumentStorage = new Mock<IStorageDataBase<MusicianInstrument>>();
        _presenter = new MusicianPresenter(_mockMusicianStorage.Object, _mockInstrumentStorage.Object, _mockMusicianInstrumentStorage.Object);
    }

    [Test]
    public async Task AddMusicianAsync_ShouldAddMusicianAndInstruments()
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
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _presenter.AddMusicianAsync(name, lastName, surname, instruments, cancellationToken);

        // Assert
        _mockMusicianStorage.Verify(m => m.AddAsync(It.Is<Musician>(musician =>
            musician.Name == name && 
            musician.Lastname == lastName && 
            musician.Surname == surname), cancellationToken), Times.Once);

        foreach (var instrument in instruments)
        {
            _mockInstrumentStorage.Verify(m => m.AddAsync(instrument, cancellationToken), Times.Once);
            _mockMusicianInstrumentStorage.Verify(m => m.AddAsync(It.Is<MusicianInstrument>(mi =>
                mi.MusicianId == result.Id &&
                mi.InstrumentId == instrument.Id), cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task DeleteMusicianAsync_ShouldDeleteMusicianAndRelations()
    {
        // Arrange
        var musician = new Musician(Guid.NewGuid(), "John", "Doe", "Smith");
        var cancellationToken = CancellationToken.None;

        // Act
        await _presenter.DeleteMusicianAsync(musician, cancellationToken);

        // Assert
        _mockMusicianStorage.Verify(m => m.DeleteAsync(It.Is<string>(condition => condition == $"Id = {musician.Id}"),
            null, cancellationToken), Times.Once);
        _mockMusicianInstrumentStorage.Verify(m => m.DeleteAsync(It.Is<string>(condition => condition == $"MusicianId = {musician.Id}"),
            null, cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetMusiciansAsync_ShouldReturnMusicians()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var musicians = new List<Musician>
        {
            new Musician(Guid.NewGuid(), "John", "Doe", "Smith"),
            new Musician(Guid.NewGuid(), "Jane", "Doe", "Smith")
        };
        _mockMusicianStorage.Setup(m => m.GetListAsync(null, null, cancellationToken))
            .ReturnsAsync(musicians);

        // Act
        var result = await _presenter.GetMusiciansAsync(cancellationToken);

        // Assert
        Assert.AreEqual(musicians.Count, result.Count);
        Assert.IsTrue(result.SequenceEqual(musicians));
        _mockMusicianStorage.Verify(m => m.GetListAsync(null, null, cancellationToken), Times.Once);
    }
}