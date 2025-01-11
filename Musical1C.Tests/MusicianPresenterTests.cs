using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests
{
    [TestFixture]
    public class MusicianPresenterTests
    {
        private Mock<IStorageDataBase<Musician>> _mockMusicianStorage;
        private Mock<IStorageDataBase<Instrument>> _mockInstrumentStorage;
        private Mock<IStorageDataBase<MusicianInstrument>> _mockMusicianInstrumentStorage;
        private Mock<IStorageDataBase<MusicianOnConcert>> _mockMusicianOnConcertStorage;
        private MusicianPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockMusicianStorage = new Mock<IStorageDataBase<Musician>>();
            _mockInstrumentStorage = new Mock<IStorageDataBase<Instrument>>();
            _mockMusicianInstrumentStorage = new Mock<IStorageDataBase<MusicianInstrument>>();
            _mockMusicianOnConcertStorage = new Mock<IStorageDataBase<MusicianOnConcert>>();
            _presenter = new MusicianPresenter(_mockMusicianStorage.Object, 
                                               _mockInstrumentStorage.Object, 
                                               _mockMusicianInstrumentStorage.Object, 
                                               _mockMusicianOnConcertStorage.Object);
        }

        // [Test]
        // public async Task AddMusicianAsync_ShouldAddMusicianWithInstrumentsAndConcerts()
        // {
        //     // Arrange
        //     var name = "John";
        //     var lastName = "Doe";
        //     var surname = "Smith";
        //     var instruments = new List<Instrument>
        //     {
        //         new Instrument { Id = Guid.NewGuid(), Name = "Guitar" },
        //         new Instrument { Id = Guid.NewGuid(), Name = "Drums" }
        //     };
        //     var concerts = new List<Concert>
        //     {
        //         new Concert { Id = Guid.NewGuid(), Name = "Rock Concert", Type = "Rock", Date = "2025-01-01" }
        //     };
        //     var token = CancellationToken.None;
        //
        //     // Act
        //     var result = await _presenter.AddMusicianAsync(name, lastName, surname, instruments, concerts, token);
        //
        //     // Assert
        //     _mockMusicianStorage.Verify(m => m.AddAsync(It.Is<Musician>(
        //         musician => musician.Name == name && musician.Lastname == lastName && musician.Surname == surname), token), Times.Once);
        //     
        //     foreach (var instrument in instruments)
        //     {
        //         _mockInstrumentStorage.Verify(i => i.AddAsync(It.Is<Instrument>(inst => inst.Name == instrument.Name), token), Times.Once);
        //         _mockMusicianInstrumentStorage.Verify(m => m.AddAsync(It.Is<MusicianInstrument>(
        //             mi => mi.MusicianId == result.Id && mi.InstrumentId == instrument.Id), token), Times.Once);
        //     }
        //
        //     foreach (var concert in concerts)
        //     {
        //         _mockMusicianOnConcertStorage.Verify(m => m.AddAsync(It.Is<MusicianOnConcert>(
        //             moc => moc.MusicianId == result.Id && moc.ConcertId == concert.Id), token), Times.Once);
        //     }
        //
        //     Assert.IsNotNull(result);
        //     Assert.AreEqual(name, result.Name);
        //     Assert.AreEqual(lastName, result.Lastname);
        //     Assert.AreEqual(surname, result.Surname);
        // }
        //
        // [Test]
        // public async Task DeleteMusicianAsync_ShouldDeleteMusicianAndRelatedData()
        // {
        //     // Arrange
        //     var musician = new Musician(Guid.NewGuid(), "John", "Doe", "Smith");
        //     var token = CancellationToken.None;
        //
        //     // Act
        //     await _presenter.DeleteMusicianAsync(musician, token);
        //
        //     // Assert
        //     _mockMusicianStorage.Verify(m => m.DeleteAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token), Times.Once);
        //     _mockMusicianInstrumentStorage.Verify(m => m.DeleteAsync(It.IsAny<Func<IQueryable<MusicianInstrument>, IQueryable<MusicianInstrument>>>(), token), Times.Once);
        // }

        [Test]
        public async Task GetMusiciansAsync_ShouldReturnMusiciansWithInstrumentsAndConcerts()
        {
            // Arrange
            var musicians = new List<Musician>
            {
                new Musician(Guid.NewGuid(), "John", "Doe", "Smith")
                {
                    MusicianInstruments = new List<MusicianInstrument>
                    {
                        new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid())
                    },
                    MusicianOnConcerts = new List<MusicianOnConcert>
                    {
                        new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid())
                    }
                }
            };
            var token = CancellationToken.None;

            _mockMusicianStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync(musicians);

            // Act
            var result = await _presenter.GetMusiciansAsync(token);

            // Assert
            Assert.AreEqual(musicians.Count, result.Count);
            _mockMusicianStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusicianByIdAsync_ShouldReturnMusicianWithInstrumentsAndConcerts()
        {
            // Arrange
            var musicianId = Guid.NewGuid();
            var musician = new Musician(musicianId, "John", "Doe", "Smith")
            {
                MusicianInstruments = new List<MusicianInstrument>
                {
                    new MusicianInstrument(Guid.NewGuid(), Guid.NewGuid())
                },
                MusicianOnConcerts = new List<MusicianOnConcert>
                {
                    new MusicianOnConcert(Guid.NewGuid(), Guid.NewGuid())
                }
            };
            var token = CancellationToken.None;

            _mockMusicianStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token))
                .ReturnsAsync(musician);

            // Act
            var result = await _presenter.GetMusicianByIdAsync(musicianId, token);

            // Assert
            Assert.AreEqual(musician.Id, result.Id);
            Assert.AreEqual(musician.Name, result.Name);
            _mockMusicianStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Musician>, IQueryable<Musician>>>(), token), Times.Once);
        }

        // [Test]
        // public void AddMusicianAsync_ShouldThrowArgumentException_WhenNameIsEmpty()
        // {
        //     // Arrange
        //     var name = string.Empty;
        //     var lastName = "Doe";
        //     var surname = "Smith";
        //     var instruments = new List<Instrument>();
        //     var concerts = new List<Concert>();
        //     var token = CancellationToken.None;
        //
        //     // Act & Assert
        //     var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
        //         await _presenter.AddMusicianAsync(name, lastName, surname, instruments, concerts, token));
        //
        //     Assert.That(exception.Message, Is.EqualTo("Name cannot be null or whitespace. (Parameter 'name')"));
        // }

        // [Test]
        // public void DeleteMusicianAsync_ShouldThrowArgumentNullException_WhenMusicianIsNull()
        // {
        //     // Arrange
        //     Musician musician = null;
        //     var token = CancellationToken.None;
        //
        //     // Act & Assert
        //     var exception = Assert.ThrowsAsync<ArgumentNullException>(async () =>
        //         await _presenter.DeleteMusicianAsync(musician, token));
        //
        //     Assert.That(exception.ParamName, Is.EqualTo("musician"));
        // }
    }
}
