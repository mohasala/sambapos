﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Samba.Domain;
using Samba.Domain.Models.Tickets;
using Samba.Infrastructure.Settings;

namespace Samba.Services
{
    public class CreditCardProcessingResult
    {
        public CreditCardProcessingResult()
        {
            PaymentType = PaymentType.CreditCard;
        }
        
        public bool IsCompleted { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreditCardProcessingData
    {
        public Ticket Ticket { get; set; }
        public decimal TenderedAmount { get; set; }
    }

    public interface ICreditCardProcessor
    {
        string Name { get; }
        void EditSettings();
        void Process(CreditCardProcessingData creditCardProcessingData);
    }

    public static class CreditCardProcessingService
    {
        private static IList<ICreditCardProcessor> CreditCardProcessors { get; set; }

        static CreditCardProcessingService()
        {
            CreditCardProcessors = new List<ICreditCardProcessor>();
        }

        public static void RegisterCreditCardProcessor(ICreditCardProcessor processor)
        {
            CreditCardProcessors.Add(processor);
        }

        public static IEnumerable<ICreditCardProcessor> GetProcessors()
        {
            return CreditCardProcessors;
        }

        public static ICreditCardProcessor GetDefaultProcessor()
        {
            var processorName = LocalSettings.DefaultCreditCardProcessorName;
            var result = CreditCardProcessors.FirstOrDefault(x => x.Name == processorName);
            return result;
        }

        public static bool CanProcessCreditCards { get { return GetDefaultProcessor() != null; } }

        public static void Process(CreditCardProcessingData ccpd)
        {
            GetDefaultProcessor().Process(ccpd);
        }
    }
}
