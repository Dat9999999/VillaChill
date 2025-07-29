using System.Linq.Expressions;
using AutoMapper;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.implements;

public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOnnxSentimentService _onnxSentimentService;
    public RatingService(IUnitOfWork unitOfWork, IMapper mapper, IOnnxSentimentService onnxSentimentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;       
        _onnxSentimentService = onnxSentimentService;      
    }
    public IEnumerable<Rating> GetAll(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "")
    {
        var ratings = _unitOfWork.Ratings.GetAll(filter, includeProperties);
        return ratings;       
    }

    public Rating GetById(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "")
    {
        throw new NotImplementedException();
    }

    public void UpdateStatus(int RatingId, string status, int villaNumber)
    {
        throw new NotImplementedException();
    }

    public void UpdatePaymentId(int RatingId, string PaymentId)
    {
        throw new NotImplementedException();
    }

    public void Add(RatingRequestDTO RatingDto)
    {
        var rating = _mapper.Map<Rating>(RatingDto);
        rating.Date = DateTime.Now;
        
        
        //sentiment prediction 
        if (rating.Comment != null)
        {
            rating.SentimentLabel = _onnxSentimentService.Predict(RatingDto.Comment);
        }
        else
        {
            if (rating.Score > 3)
            {
                rating.SentimentLabel = "Positive";
            }
            else if (rating.Score < 3)
            {
                rating.SentimentLabel = "Negative";           
            }
            else rating.SentimentLabel = "Neutral";       
        }
        
        //save rating
        _unitOfWork.Ratings.Add(rating);
        _unitOfWork.Save();   
    }
}