﻿using HotelManagement.Entities;
using HotelManagement.Helpers;
using HotelManagement.Interfaces;

namespace HotelManagement.Repositories
{
    public class HotelFileRepository(string filePath) : IHotelRepository
    {
        public async Task<Hotel?> GetHotelAsync(string hotelId)
        {
            var hotels = await GetHotelsAsync();
            return hotels.FirstOrDefault(h => string.Equals(h.Id, hotelId, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            var hotels = await JsonDataLoader.LoadData<IEnumerable<Hotel>>(filePath);
            return hotels;
        }
    }
}
