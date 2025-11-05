using CompanyDealer.BLL.DTOs.FeedbackDTOs;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.FeedbackRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackResponseDto>> GetAllAsync();
        Task<FeedbackResponseDto> GetByIdAsync(Guid id);
        Task<FeedbackResponseDto> CreateAsync(FeedbackRequestDto dto);
        Task<bool> UpdateAsync(Guid id, FeedbackRequestDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;

        public FeedbackService(IFeedbackRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<FeedbackResponseDto>> GetAllAsync()
        {
            var feedbacks = await _repo.GetAllAsync();
            return feedbacks.Select(f => new FeedbackResponseDto
            {
                Id = f.Id,
                Content = f.Content,
                Rating = f.Rating,
                SubmissionDate = f.SubmissionDate,
                DealerId = f.DealerId,
                CustomerId = f.CustomerId
            });
        }

        public async Task<FeedbackResponseDto> GetByIdAsync(Guid id)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return null;
            return new FeedbackResponseDto
            {
                Id = f.Id,
                Content = f.Content,
                Rating = f.Rating,
                SubmissionDate = f.SubmissionDate,
                DealerId = f.DealerId,
                CustomerId = f.CustomerId
            };
        }

        public async Task<FeedbackResponseDto> CreateAsync(FeedbackRequestDto dto)
        {
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Content = dto.Content,
                Rating = dto.Rating,
                SubmissionDate = DateTime.UtcNow,
                DealerId = dto.DealerId,
                CustomerId = dto.CustomerId
            };
            await _repo.AddAsync(feedback);
            return new FeedbackResponseDto
            {
                Id = feedback.Id,
                Content = feedback.Content,
                Rating = feedback.Rating,
                SubmissionDate = feedback.SubmissionDate,
                DealerId = feedback.DealerId,
                CustomerId = feedback.CustomerId
            };
        }

        public async Task<bool> UpdateAsync(Guid id, FeedbackRequestDto dto)
        {
            var feedback = await _repo.GetByIdAsync(id);
            if (feedback == null) return false;
            feedback.Content = dto.Content;
            feedback.Rating = dto.Rating;
            feedback.DealerId = dto.DealerId;
            feedback.CustomerId = dto.CustomerId;
            await _repo.UpdateAsync(feedback);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var feedback = await _repo.GetByIdAsync(id);
            if (feedback == null) return false;
            await _repo.DeleteAsync(feedback);
            return true;
        }
    }
}
