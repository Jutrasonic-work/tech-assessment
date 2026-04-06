import { apiJson } from "@/shared/api/http";
import type {
    CreateCourseBody,
    CreateCourseResponse,
    GetCourseByIdResponse,
    GetCoursesItem,
    UpdateCourseBody,
} from "@/shared/api/types";

export function getCourses(): Promise<GetCoursesItem[]> {
    return apiJson<GetCoursesItem[]>("/api/admin/courses");
}

export function getCourseById(courseId: number): Promise<GetCourseByIdResponse> {
    return apiJson<GetCourseByIdResponse>(`/api/admin/courses/${courseId}`);
}

export function createCourse(body: CreateCourseBody): Promise<CreateCourseResponse> {
    return apiJson<CreateCourseResponse>("/api/admin/courses", {
        method: "POST",
        body: JSON.stringify(body),
    });
}

export function updateCourse(courseId: number, body: UpdateCourseBody): Promise<void> {
    return apiJson<void>(`/api/admin/courses/${courseId}`, {
        method: "PUT",
        body: JSON.stringify(body),
    });
}

export function deleteCourse(courseId: number): Promise<void> {
    return apiJson<void>(`/api/admin/courses/${courseId}`, {
        method: "DELETE",
    });
}
