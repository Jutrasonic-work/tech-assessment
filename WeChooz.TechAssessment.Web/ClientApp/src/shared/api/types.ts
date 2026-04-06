/** Aligné sur les enums JSON (valeurs numériques). */
export const CseAudience = {
    DelegateElu: 0,
    President: 1,
} as const;
export type CseAudience = (typeof CseAudience)[keyof typeof CseAudience];

export const SessionDeliveryMode = {
    InPerson: 0,
    Remote: 1,
} as const;
export type SessionDeliveryMode = (typeof SessionDeliveryMode)[keyof typeof SessionDeliveryMode];

export type LoginClaimDto = { type: string; value: string };

export type LoginResponse = {
    login: string;
    claims: LoginClaimDto[];
};

export type GetPublicSessionsItem = {
    sessionId: number;
    courseName: string;
    shortDescription: string;
    cseAudience: CseAudience;
    startDate: string;
    durationDays: number;
    deliveryMode: SessionDeliveryMode;
    remainingSeats: number;
    trainerFirstName: string;
    trainerLastName: string;
};

export type GetPublicSessionDetailResponse = GetPublicSessionsItem & {
    longDescriptionHtml: string;
};

export type GetCoursesItem = {
    courseId: number;
    name: string;
    cseAudience: CseAudience;
    durationDays: number;
    maxCapacity: number;
};

export type GetCourseByIdResponse = {
    courseId: number;
    name: string;
    shortDescription: string;
    longDescriptionMarkdown: string;
    durationDays: number;
    cseAudience: CseAudience;
    maxCapacity: number;
    trainerFirstName: string;
    trainerLastName: string;
};

export type CreateCourseBody = {
    name: string;
    shortDescription: string;
    longDescriptionMarkdown: string;
    durationDays: number;
    cseAudience: CseAudience;
    maxCapacity: number;
    trainerFirstName: string;
    trainerLastName: string;
};

export type UpdateCourseBody = CreateCourseBody;

export type CreateCourseResponse = { courseId: number };

export type GetAdminSessionsItem = {
    sessionId: number;
    courseId: number;
    courseName: string;
    startDate: string;
    deliveryMode: SessionDeliveryMode;
    participantCount: number;
    maxCapacity: number;
};

export type GetAdminSessionByIdResponse = GetAdminSessionsItem;

export type CreateSessionBody = {
    courseId: number;
    startDate: string;
    deliveryMode: SessionDeliveryMode;
};

export type UpdateSessionBody = {
    courseId: number;
    startDate: string;
    deliveryMode: SessionDeliveryMode;
};

export type CreateSessionResponse = { sessionId: number };

export type GetParticipantsBySessionItem = {
    participantId: number;
    sessionId: number;
    lastName: string;
    firstName: string;
    email: string;
    companyName: string;
};

export type AddParticipantBody = {
    lastName: string;
    firstName: string;
    email: string;
    companyName: string;
};

export type AddParticipantResponse = { participantId: number };

export type UpdateParticipantBody = AddParticipantBody & { sessionId: number };
