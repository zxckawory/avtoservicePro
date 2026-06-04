--
-- PostgreSQL database dump
--

\restrict MhmhGbwDi47MpNPYUA7Cfxohv8uFSoq8YzKU7v1fOg0wbMth0gm9WPsVur0IPsw

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2026-05-04 09:44:41

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 220 (class 1259 OID 17880)
-- Name: Car; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Car" (
    "Id" integer NOT NULL,
    "CarName" text NOT NULL,
    "CarNumber" text NOT NULL,
    "UserId" integer NOT NULL,
    "CarTypeId" integer NOT NULL,
    "HorsePower" integer NOT NULL,
    "EngineVolume" numeric NOT NULL,
    "FuelTypeId" integer NOT NULL,
    "Year" integer NOT NULL,
    "Mileage" integer NOT NULL
);


--
-- TOC entry 225 (class 1259 OID 18074)
-- Name: CarType; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."CarType" (
    "Id" integer NOT NULL,
    "Type" text NOT NULL
);


--
-- TOC entry 226 (class 1259 OID 18089)
-- Name: FuelType; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."FuelType" (
    "Id" integer NOT NULL,
    "Type" text NOT NULL
);


--
-- TOC entry 222 (class 1259 OID 17908)
-- Name: Order; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Order" (
    "Id" integer NOT NULL,
    "OrderDayTime" timestamp without time zone NOT NULL,
    "Description" text,
    "Image" text,
    "CarId" integer NOT NULL
);


--
-- TOC entry 223 (class 1259 OID 17921)
-- Name: OrderService; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."OrderService" (
    "OrderId" integer NOT NULL,
    "ServiceId" integer CONSTRAINT "OrderService_''ServiceId""_not_null" NOT NULL
);


--
-- TOC entry 224 (class 1259 OID 17949)
-- Name: Role; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Role" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- TOC entry 219 (class 1259 OID 17870)
-- Name: Service; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Service" (
    "Id" integer NOT NULL,
    "ServiceName" text NOT NULL,
    "ServiceCost" integer NOT NULL
);


--
-- TOC entry 221 (class 1259 OID 17890)
-- Name: User; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."User" (
    "Id" integer CONSTRAINT "Client_Id_not_null" NOT NULL,
    "Name" text CONSTRAINT "Client_Name_not_null" NOT NULL,
    "Login" text CONSTRAINT "Client_Login_not_null" NOT NULL,
    "Password" text CONSTRAINT "Client_Password_not_null" NOT NULL,
    "PhoneNumber" text CONSTRAINT "Client_PhoneNumber_not_null" NOT NULL,
    "RoleId" integer NOT NULL
);


--
-- TOC entry 5024 (class 0 OID 17880)
-- Dependencies: 220
-- Data for Name: Car; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Car" VALUES (1, 'BMW M5', 'A123BC77', 1, 1, 600, 4.4, 1, 2020, 45000);
INSERT INTO public."Car" VALUES (2, 'Audi A6', 'B456DE77', 2, 1, 340, 3.0, 1, 2019, 60000);
INSERT INTO public."Car" VALUES (3, 'Toyota Camry', 'C789FG77', 3, 1, 249, 2.5, 1, 2021, 30000);
INSERT INTO public."Car" VALUES (4, 'Volkswagen Golf', 'D321HI77', 1, 2, 150, 1.4, 1, 2018, 80000);
INSERT INTO public."Car" VALUES (5, 'Ford Focus', 'E654JK77', 2, 2, 125, 1.6, 1, 2017, 90000);
INSERT INTO public."Car" VALUES (6, 'Hyundai Tucson', 'F987LM77', 3, 7, 190, 2.0, 2, 2022, 20000);
INSERT INTO public."Car" VALUES (7, 'Kia Sportage', 'G159NO77', 1, 7, 184, 2.4, 1, 2021, 25000);
INSERT INTO public."Car" VALUES (8, 'Tesla Model 3', 'H753PQ77', 2, 1, 283, 0.0, 3, 2023, 10000);
INSERT INTO public."Car" VALUES (9, 'Nissan X-Trail', 'J852RS77', 3, 6, 171, 2.5, 1, 2020, 55000);
INSERT INTO public."Car" VALUES (10, 'Mercedes-Benz E-Class', 'K951TU77', 1, 1, 367, 3.0, 1, 2019, 40000);


--
-- TOC entry 5029 (class 0 OID 18074)
-- Dependencies: 225
-- Data for Name: CarType; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."CarType" VALUES (1, 'Седан');
INSERT INTO public."CarType" VALUES (2, 'Хэтчбек');
INSERT INTO public."CarType" VALUES (3, 'Универсал');
INSERT INTO public."CarType" VALUES (4, 'Купе');
INSERT INTO public."CarType" VALUES (5, 'Кабриолет');
INSERT INTO public."CarType" VALUES (6, 'Внедорожник');
INSERT INTO public."CarType" VALUES (7, 'Кроссовер');
INSERT INTO public."CarType" VALUES (8, 'Минивэн');
INSERT INTO public."CarType" VALUES (9, 'Пикап');
INSERT INTO public."CarType" VALUES (10, 'Лимузин');


--
-- TOC entry 5030 (class 0 OID 18089)
-- Dependencies: 226
-- Data for Name: FuelType; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."FuelType" VALUES (1, 'Бензин');
INSERT INTO public."FuelType" VALUES (2, 'Дизель');
INSERT INTO public."FuelType" VALUES (3, 'Электричество');
INSERT INTO public."FuelType" VALUES (4, 'Гибрид');
INSERT INTO public."FuelType" VALUES (5, 'Газ (LPG)');
INSERT INTO public."FuelType" VALUES (6, 'Газ (CNG)');


--
-- TOC entry 5026 (class 0 OID 17908)
-- Dependencies: 222
-- Data for Name: Order; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Order" VALUES (1, '2026-03-20 10:00:00', 'Плановое ТО', NULL, 1);
INSERT INTO public."Order" VALUES (2, '2026-03-21 12:30:00', 'Проблема с двигателем', NULL, 2);
INSERT INTO public."Order" VALUES (3, '2026-03-22 09:15:00', 'Замена колес', NULL, 3);


--
-- TOC entry 5027 (class 0 OID 17921)
-- Dependencies: 223
-- Data for Name: OrderService; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."OrderService" VALUES (1, 1);
INSERT INTO public."OrderService" VALUES (1, 2);
INSERT INTO public."OrderService" VALUES (2, 2);
INSERT INTO public."OrderService" VALUES (2, 4);
INSERT INTO public."OrderService" VALUES (3, 3);


--
-- TOC entry 5028 (class 0 OID 17949)
-- Dependencies: 224
-- Data for Name: Role; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Role" VALUES (1, 'Client');
INSERT INTO public."Role" VALUES (2, 'Admin');


--
-- TOC entry 5023 (class 0 OID 17870)
-- Dependencies: 219
-- Data for Name: Service; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Service" VALUES (1, 'Замена масла', 100);
INSERT INTO public."Service" VALUES (2, 'Диагностика', 50);
INSERT INTO public."Service" VALUES (3, 'Шиномонтаж', 80);
INSERT INTO public."Service" VALUES (4, 'Покраска', 300);


--
-- TOC entry 5025 (class 0 OID 17890)
-- Dependencies: 221
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."User" VALUES (1, 'Иван Иванов', 'ivan', '1234', '+491111111111', 1);
INSERT INTO public."User" VALUES (2, 'Петр Петров', 'petr', '1234', '+492222222222', 1);
INSERT INTO public."User" VALUES (3, 'Администратор', 'admin', 'admin', '+493333333333', 2);
INSERT INTO public."User" VALUES (4, 'test', 'test', 'test', '+79921741172', 1);


--
-- TOC entry 4856 (class 2606 OID 17889)
-- Name: Car _car__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Car"
    ADD CONSTRAINT _car__pk PRIMARY KEY ("Id");


--
-- TOC entry 4866 (class 2606 OID 18082)
-- Name: CarType _cartype__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."CarType"
    ADD CONSTRAINT _cartype__pk PRIMARY KEY ("Id");


--
-- TOC entry 4858 (class 2606 OID 17901)
-- Name: User _client__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT _client__pk PRIMARY KEY ("Id");


--
-- TOC entry 4868 (class 2606 OID 18097)
-- Name: FuelType _fueltype__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."FuelType"
    ADD CONSTRAINT _fueltype__pk PRIMARY KEY ("Id");


--
-- TOC entry 4860 (class 2606 OID 17915)
-- Name: Order _order__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT _order__pk PRIMARY KEY ("Id");


--
-- TOC entry 4862 (class 2606 OID 17927)
-- Name: OrderService _orderservice__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderService"
    ADD CONSTRAINT _orderservice__pk PRIMARY KEY ("OrderId", "ServiceId");


--
-- TOC entry 4864 (class 2606 OID 17957)
-- Name: Role _role__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT _role__pk PRIMARY KEY ("Id");


--
-- TOC entry 4854 (class 2606 OID 17879)
-- Name: Service _service__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Service"
    ADD CONSTRAINT _service__pk PRIMARY KEY ("Id");


--
-- TOC entry 4874 (class 2606 OID 18064)
-- Name: OrderService _orderservice__order_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderService"
    ADD CONSTRAINT _orderservice__order_fk FOREIGN KEY ("OrderId") REFERENCES public."Order"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4875 (class 2606 OID 18069)
-- Name: OrderService _orderservice__service_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderService"
    ADD CONSTRAINT _orderservice__service_fk FOREIGN KEY ("ServiceId") REFERENCES public."Service"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4869 (class 2606 OID 18098)
-- Name: Car car__fueltype__fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Car"
    ADD CONSTRAINT car__fueltype__fk FOREIGN KEY ("FuelTypeId") REFERENCES public."FuelType"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4870 (class 2606 OID 18084)
-- Name: Car car_cartype_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Car"
    ADD CONSTRAINT car_cartype_fk FOREIGN KEY ("CarTypeId") REFERENCES public."CarType"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4871 (class 2606 OID 17965)
-- Name: Car car_user_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Car"
    ADD CONSTRAINT car_user_fk FOREIGN KEY ("UserId") REFERENCES public."User"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4873 (class 2606 OID 17973)
-- Name: Order order_car_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT order_car_fk FOREIGN KEY ("CarId") REFERENCES public."Car"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4872 (class 2606 OID 17959)
-- Name: User user_role_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT user_role_fk FOREIGN KEY ("RoleId") REFERENCES public."Role"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


-- Completed on 2026-05-04 09:44:42

--
-- PostgreSQL database dump complete
--

\unrestrict MhmhGbwDi47MpNPYUA7Cfxohv8uFSoq8YzKU7v1fOg0wbMth0gm9WPsVur0IPsw

