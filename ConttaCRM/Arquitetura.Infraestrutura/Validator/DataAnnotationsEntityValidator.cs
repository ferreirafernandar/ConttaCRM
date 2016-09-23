using System.Collections.Generic;
using System.Linq;
using Arquitetura.Dominio.Base;

namespace Arquitetura.Infraestrutura.Validator
{
    public class DataAnnotationsEntityValidator : IEntityValidator
    {
        #region Private Methods

        /// <summary>
        /// Get erros if object implement IValidator
        /// </summary>
        /// <typeparam name="TEntity">The typeof entity</typeparam>
        /// <param name="item">The item to validate</param>
        /// <param name="errors">A collection of current errors</param>
        void SetValidatableObjectErrors<TEntity>(TEntity item, List<ValidationResult> errors) where TEntity : class
        {
            if (typeof(IValidator).IsAssignableFrom(typeof(TEntity)))
            {
                var validationResults = ((IValidator)item).Validate();

                foreach (var validationResult in validationResults)
                {
                    errors.Add(new ValidationResult(validationResult.First(), validationResult.Last()));
                }
            }
        }

        #endregion

        #region IEntityValidator Members


        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/>
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/></typeparam>
        /// <param name="item"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/></returns>
        public bool IsValid<TEntity>(TEntity item) where TEntity : class
        {

            if (item == null)
                return false;

            List<ValidationResult> validationErrors = new List<ValidationResult>();

            SetValidatableObjectErrors(item, validationErrors);

            return !validationErrors.Any();
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/>
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/></typeparam>
        /// <param name="item"><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Infrastructure.CrossCutting.Validator.IEntityValidator"/></returns>
        public IEnumerable<ValidationResult> GetInvalidMessages<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
                return null;

            List<ValidationResult> validationErrors = new List<ValidationResult>();

            SetValidatableObjectErrors(item, validationErrors);

            return validationErrors;
        }

        #endregion
    }
}
