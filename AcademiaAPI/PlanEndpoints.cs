namespace AcademiaAPI
{
    public static class PlanEndpoints
    {
        public static void MapPlanEndpoints(this WebApplication app) { 

            var planService = new Aplication.Services.PlanService();
            app.MapGet("/planes", () =>
            {
                var planes = planService.GetAll();
                return Results.Ok(planes);
            });
            app.MapGet("/planes/{id:int}", (int id) =>
            {
                var plan = planService.GetById(id);
                return plan == null ? Results.NotFound() : Results.Ok(plan);
            });
            app.MapPost("/planes", (DTOs.PlanDto planDto) =>
            {
                planService.Add(planDto);
                return Results.Created($"/planes/{planDto.Id}", planDto);
            });
            app.MapPut("/planes/{id:int}", (int id, DTOs.PlanDto planDto) =>
            {
                if (id != planDto.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var existingPlan = planService.GetById(id);
                if (existingPlan == null)
                {
                    return Results.NotFound();
                }
                planService.Update(planDto);
                return Results.NoContent();
            });
            
            app.MapDelete("/planes/{id:int}", (int id) =>
            {
                var existingPlan = planService.GetById(id);
                if (existingPlan == null)
                {
                    return Results.NotFound();
                }
                
                planService.Delete(id);
                return Results.NoContent();
            });
        }
    }
}
