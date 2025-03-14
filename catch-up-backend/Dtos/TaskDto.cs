﻿using catch_up_backend.Enums;
using catch_up_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public Guid? NewbieId { get; set; }
        public Guid? AssigningId { get; set; }
        public int TaskContentId { get; set; }
        public int? RoadMapPointId { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public DateTime? Deadline { get; set; }
        public double SpendTime { get; set; }
        public int Priority { get; set; }
        public int? Rate { get; set; }
        public TaskDto() { }
        public TaskDto(TaskModel task)
        {
            Id = task.Id;
            NewbieId = task.NewbieId;
            AssigningId = task.AssigningId;
            TaskContentId = task.TaskContentId;
            RoadMapPointId = task.RoadMapPointId;
            Status = task.Status;
            AssignmentDate = task.AssignmentDate;
            FinalizationDate = task.FinalizationDate;
            Deadline = task.Deadline;
            SpendTime = task.SpendTime;
            Priority = task.Priority;
            Rate = task.Rate;
        }
    }
}
